using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using PregnaCare.Api.Models.Requests.PaymentRequestModel;
using PregnaCare.Api.Models.Responses.PaymentResponseModel;
using PregnaCare.Common.Constants;
using PregnaCare.Common.Enums;
using PregnaCare.Core.Models;
using PregnaCare.Infrastructure.Data;
using PregnaCare.Services.Interfaces;
using PregnaCare.Utils;

namespace PregnaCare.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly PregnaCareAppDbContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="context"></param>
        public PaymentService(IConfiguration configuration, PregnaCareAppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public string CreatePaymentUrl(PaymentRequest request, HttpContext context)
        {
            var user = _context.Users.AsNoTracking().FirstOrDefault(x => x.Id == request.UserId);
            var membershipPlan = _context.MembershipPlans.AsNoTracking().FirstOrDefault(x => x.Id == request.MembershipPlanId);
            if (user == null || membershipPlan == null) return string.Empty;

            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["Vnpay:TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.UtcNow.Ticks.ToString();
            var pay = new VnpayUtils();
            var urlCallBack = _configuration["Vnpay:ReturnUrl"];

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)membershipPlan.Price * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{membershipPlan.PlanName}: {membershipPlan.Description ?? string.Empty} - {user.Email}");
            pay.AddRequestData("vnp_OrderType", "Upgrade MembershipPlan");
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl = pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            return paymentUrl;
        }

        public async Task<PaymentInitiationResponse> InitiatePayment(PaymentInitiationRequest request, HttpContext context)
        {
            var response = new PaymentInitiationResponse { Success = false };
            var membershipPlan = await _context.MembershipPlans.FirstOrDefaultAsync(p => p.Id == request.MembershipPlanId);
            if (membershipPlan == null)
            {
                response.MessageId = Messages.E00002;
                response.Message = Messages.GetMessageById(Messages.E00002);
                return response;
            }

            var existingActivePlan = await _context.UserMembershipPlans
                .FirstOrDefaultAsync(p => p.UserId == request.UserId &&
                                         p.MembershipPlanId == request.MembershipPlanId &&
                                         (bool)p.IsActive);

            if (existingActivePlan != null && existingActivePlan.Status == StatusEnum.Completed.ToString())
            {
                response.MessageId = Messages.E00014;
                response.Message = Messages.GetMessageById(Messages.E00014);
                return response;
            }

            var pendingPayment = await _context.UserMembershipPlans
                .FirstOrDefaultAsync(p => p.UserId == request.UserId &&
                                         p.MembershipPlanId == request.MembershipPlanId &&
                                         p.Status == StatusEnum.InProgress.ToString());

            UserMembershipPlan userMembershipPlan;
            if (pendingPayment != null)
            {
                userMembershipPlan = pendingPayment;
                userMembershipPlan.StatusChangedAt = DateTime.Now;
            }
            else
            {
                // Create new payment record with InProgress status
                userMembershipPlan = new UserMembershipPlan
                {
                    UserId = request.UserId,
                    MembershipPlanId = request.MembershipPlanId,
                    Price = membershipPlan.Price,
                    Status = StatusEnum.InProgress.ToString(),
                    StatusChangedAt = DateTime.Now,
                    IsActive = false,
                    CreatedAt = DateTime.Now
                };

                _ = _context.UserMembershipPlans.Add(userMembershipPlan);
                _ = await _context.SaveChangesAsync();
            }

            var vnpayUrl = CreateVNPayURL(context, membershipPlan, userMembershipPlan, request.UserEmail);

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            response.Response = new()
            {
                PaymentId = userMembershipPlan.Id,
                PaymentUrl = vnpayUrl,
            };

            return response;
        }

        public PaymentResponse PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnpayUtils();
            var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);
            if (response.Success)
            {
                var pattern = @"^(.*?): .* - (.*?)$";
                var match = Regex.Match(response.OrderDescription, pattern);

                if (match.Success)
                {
                    var planName = match.Groups[1].Value;
                    var email = match.Groups[2].Value;

                    var membershipPlan = _context.MembershipPlans.AsNoTracking().FirstOrDefault(x => x.PlanName == planName);
                    var user = _context.Users.AsNoTracking().FirstOrDefault(x => x.Email.ToLower() == email.ToLower());

                    if (membershipPlan == null || user == null)
                    {
                        response.Success = false;
                        return response;
                    }

                    var userRole = _context.UserRoles.FirstOrDefault(x => x.UserId == user.Id);

                    if (userRole != null)
                    {
                        var role = _context.Roles.FirstOrDefault(x => x.RoleName == RoleEnum.Member.ToString());
                        if (role != null)
                        {
                            userRole.RoleId = role.Id;
                            _ = _context.UserRoles.Update(userRole);
                        }
                    }

                    var userMembershipPlan = new UserMembershipPlan
                    {
                        UserId = user.Id,
                        MembershipPlanId = membershipPlan.Id,
                        IsActive = true,
                        Status = StatusEnum.Completed.ToString(),
                        ExpiryDate = DateTime.Now.AddDays(30),
                        Price = membershipPlan.Price
                    };

                    _ = _context.UserMembershipPlans.Add(userMembershipPlan);
                    _ = _context.SaveChanges();
                    return response;
                }
            }

            return response;
        }

        public async Task<PaymentCallbackResponse> ProcessCallback(IQueryCollection queryParams)
        {
            var response = new PaymentCallbackResponse { Success = false };
            var vnpay = new VnpayUtils();
            var vnpayResponse = vnpay.GetFullResponseDataV2(queryParams, _configuration["Vnpay:HashSecret"]);
            if (!queryParams.TryGetValue("vnp_TxnRef", out var txnRefValues))
            {
                response.MessageId = Messages.E00000;
                response.Message = Messages.GetMessageById(Messages.E00000);
                return response;
            }

            var responseCode = queryParams["vnp_ResponseCode"].ToString();

            var txnRef = txnRefValues.FirstOrDefault();
            var userMembershipPlan = await _context.UserMembershipPlans
                .Include(p => p.MembershipPlan)
                .FirstOrDefaultAsync(p => p.PaymentReference == txnRef);

            if (userMembershipPlan == null)
            {
                response.MessageId = Messages.E00013;
                response.Message = Messages.GetMessageById(Messages.E00013);
                return response;
            }

            StatusEnum newStatus;
            string statusNotes = string.Empty;
            bool isActive = false;
            DateTime? expiryDate = null;

            switch (responseCode)
            {
                case "00": // Success
                    newStatus = StatusEnum.Completed;
                    statusNotes = "Payment completed successfully";
                    isActive = true;
                    expiryDate = DateTime.Now.AddDays(30); // Or use plan duration
                    break;

                case "24": // Customer cancelled
                    newStatus = StatusEnum.Cancelled;
                    statusNotes = "Payment cancelled by user";
                    break;

                case "51": // Insufficient funds
                    newStatus = StatusEnum.Failed;
                    statusNotes = "Payment failed: Insufficient funds";
                    break;

                default:
                    newStatus = StatusEnum.Failed;
                    statusNotes = $"Payment failed with code: {responseCode}";
                    break;
            }

            userMembershipPlan.Status = newStatus.ToString();
            userMembershipPlan.StatusChangedAt = DateTime.Now;
            userMembershipPlan.StatusNotes = statusNotes;
            userMembershipPlan.PaymentErrorCode = responseCode != "00" ? responseCode : null;
            userMembershipPlan.PaymentReference = queryParams["vnp_TransactionNo"].ToString();

            if (isActive)
            {
                userMembershipPlan.IsActive = true;
                userMembershipPlan.ActivatedAt = DateTime.Now;
                userMembershipPlan.ExpiryDate = expiryDate;

                // Handle existing active plans if this is an upgrade
                if (newStatus == StatusEnum.Completed)
                {
                    await HandleExistingPlans(userMembershipPlan.UserId, userMembershipPlan.Id);
                }

                // Update user role if needed
                await UpdateUserRole(userMembershipPlan.UserId);
            }

            _ = await _context.SaveChangesAsync();


            response.Success = vnpayResponse.Success;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            response.Response = new()
            {
                Status = newStatus.ToString(),
                PaymentId = txnRef,
                ResponseCode = responseCode,
            };

            return response;
        }

        private string CreateVNPayURL(HttpContext context, MembershipPlan membershipPlan, UserMembershipPlan userMembershipPlan, string email)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["Vnpay:TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var uniqueTxnRef = DateTime.UtcNow.Ticks.ToString();
            var pay = new VnpayUtils();
            var urlCallBack = _configuration["Vnpay:ReturnUrl"];

            userMembershipPlan.PaymentReference = uniqueTxnRef;
            _ = _context.SaveChanges();

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)userMembershipPlan.Price * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{membershipPlan.PlanName}: Payment for plan - {email}");
            pay.AddRequestData("vnp_OrderType", "Upgrade MembershipPlan");
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", uniqueTxnRef);

            return pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);
        }

        // Helper method to handle existing plans for upgrades
        private async Task HandleExistingPlans(Guid userId, Guid currentPlanId)
        {
            var existingPlans = await _context.UserMembershipPlans
                .Where(p => p.UserId == userId &&
                          p.Id != currentPlanId &&
                         (bool)p.IsActive)
                .ToListAsync();

            foreach (var plan in existingPlans)
            {
                plan.IsActive = false;
                plan.Status = StatusEnum.Superseded.ToString();
                plan.StatusChangedAt = DateTime.Now;
                plan.ExpiryDate = DateTime.Now;
                plan.StatusNotes = "Superseded by new plan";
            }
        }

        // Helper method to update user role
        private async Task UpdateUserRole(Guid userId)
        {
            var userRole = await _context.UserRoles
                .FirstOrDefaultAsync(r => r.UserId == userId);

            if (userRole != null)
            {
                var memberRole = await _context.Roles
                    .FirstOrDefaultAsync(r => r.RoleName == RoleEnum.Member.ToString());

                if (memberRole != null)
                {
                    userRole.RoleId = memberRole.Id;
                    _ = _context.UserRoles.Update(userRole);
                }
            }
        }

        // Background job to handle expired payments
        public async Task HandleExpiredPayments()
        {
            // Find all payments in InProgress status older than 24 hours
            var cutoffTime = DateTime.Now.AddHours(-24);
            var expiredPayments = await _context.UserMembershipPlans
                .Where(p => p.Status == StatusEnum.InProgress.ToString() &&
                          p.CreatedAt < cutoffTime)
                .ToListAsync();

            foreach (var payment in expiredPayments)
            {
                payment.Status = StatusEnum.Expired.ToString();
                payment.StatusChangedAt = DateTime.Now;
                payment.StatusNotes = "Payment session expired";
            }

            _ = await _context.SaveChangesAsync();
        }
    }
}