using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using PregnaCare.Api.Models.Requests.PaymentRequestModel;
using PregnaCare.Api.Models.Responses.PaymentResponseModel;
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
            var tick = DateTime.Now.Ticks.ToString();
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

                    var userMembershipPlan = new UserMembershipPlan
                    {
                        UserId = user.Id,
                        MembershipPlanId = membershipPlan.Id,
                        IsActive = false,
                        Status = StatusEnum.Completed.ToString()
                    };

                    _ = _context.UserMembershipPlans.Add(userMembershipPlan);
                    _ = _context.SaveChanges();
                    return response;
                }
            }

            return response;
        }
    }
}