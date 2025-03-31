using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.BackgroundServices
{
    public class PlanNotificationService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public PlanNotificationService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var notificationRepository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
                    var userMembershipPlanService = scope.ServiceProvider.GetRequiredService<IUserMembershipPlanSerivce>();

                    // Call GetExpiringUserMembershipPlans
                    var expiringPlansResponse = await userMembershipPlanService.GetExpiringUserMembershipPlans();

                    if (expiringPlansResponse.Success && expiringPlansResponse.Response != null)
                    {
                        foreach (var plan in expiringPlansResponse.Response)
                        {
                            var notification = new Notification
                            {
                                ReceiverId = plan.UserId,
                                Title = "Membership Expiry Reminder",
                                Message = "Your membership plan will expire tomorrow. Please renew to avoid service interruption.",
                                CreatedAt = DateTime.Now
                            };

                            await notificationRepository.AddAsync(notification);
                        }

                        await unitOfWork.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in PlanNotificationService: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
