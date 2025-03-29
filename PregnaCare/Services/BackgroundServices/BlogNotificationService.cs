using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PregnaCare.Services.BackgroundServices
{
    public class BlogNotificationService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private DateTime _lastRunTime = DateTime.Now;
        public BlogNotificationService(IServiceScopeFactory serviceScopeFactory)
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
                    var blogRepository = scope.ServiceProvider.GetRequiredService<IBlogRepository>();
                    var userRepository = scope.ServiceProvider.GetRequiredService<IAccountRepository>();
                    var notificationRepository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    var currentTime = DateTime.Now;
                    var rejectedBlogs = await blogRepository.FindAsync(b => b.Status == "Rejected"
                        && b.UpdatedAt > _lastRunTime
                        && b.UpdatedAt <= currentTime);


                    if (rejectedBlogs.Any())
                    {
                        foreach (var blog in rejectedBlogs)
                        {
                            var user = await userRepository.GetByIdAsync(blog.UserId);
                            if (user != null)
                            {
                                var notification = new Notification
                                {
                                    Id = Guid.NewGuid(),
                                    ReceiverId = user.Id,
                                    Title = "Reject your blog",
                                    Message = $"Your blog '{blog.Heading}' was rejected by Admin.",
                                    CreatedAt = currentTime,
                                    IsRead = false
                                };

                                await notificationRepository.AddAsync(notification);
                            }
                        }

                        await unitOfWork.SaveChangesAsync();
                    }
                    _lastRunTime = currentTime;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in BlogNotificationService: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
