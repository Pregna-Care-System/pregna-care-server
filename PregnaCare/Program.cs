
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PregnaCare.Core.Repositories.Implementations;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.Data;
using PregnaCare.Infrastructure.Hubs;
using PregnaCare.Infrastructure.UnitOfWork;
using PregnaCare.Middlewares;
using PregnaCare.Services.BackgroundServices;
using PregnaCare.Services.Implementations;
using PregnaCare.Services.Interfaces;
using QuestPDF.Infrastructure;

namespace PregnaCare
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            _ = Env.Load();

            QuestPDF.Settings.License = LicenseType.Community;

            var builder = WebApplication.CreateBuilder(args);

            // Get connection string
            var authDbConnection = builder.Configuration["ConnectionStrings:AuthDbConnection"];
            var applicationDbConnection = builder.Configuration["ConnectionStrings:ApplicationDbConnection"];

            // Add services to the container.
            _ = builder.Services.AddDbContext<PregnaCareAppDbContext>(options => options.UseSqlServer(applicationDbConnection));
            _ = builder.Services.AddDbContext<PregnaCareAuthDbContext>(options => options.UseSqlServer(authDbConnection));

            _ = builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            _ = builder.Services.AddScoped<IMembershipPlansRepository, MembershipPlansRepository>();
            _ = builder.Services.AddScoped<IFeatureRepository, FeatureRepository>();
            _ = builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            _ = builder.Services.AddScoped<IUserMembershipPlanRepository, UserMembershipPlanRepository>();
            _ = builder.Services.AddScoped<IReminderRepository, ReminderRepository>();
            _ = builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            _ = builder.Services.AddScoped<IBlogRepository, BlogRepository>();
            _ = builder.Services.AddScoped<ITagRepository, TagRepository>();
            _ = builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            _ = builder.Services.AddScoped<IBlogTagRepository, BlogTagRepository>();
            _ = builder.Services.AddScoped<IFeedBackRepository, FeedBackRepository>();
            _ = builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            _ = builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            _ = builder.Services.AddScoped<IAuthService, AuthService>();
            _ = builder.Services.AddScoped<ITokenService, TokenService>();
            _ = builder.Services.AddScoped<IEmailService, EmailService>();
            _ = builder.Services.AddScoped<IPasswordService, PasswordService>();
            _ = builder.Services.AddScoped<IMembershipPlansService, MembershipPlansService>();
            _ = builder.Services.AddScoped<IFeatureService, FeatureService>();
            _ = builder.Services.AddScoped<IPregnancyRecordService, PregnancyRecordService>();
            _ = builder.Services.AddScoped<IUserMembershipPlanSerivce, UserMembershipPlanService>();
            _ = builder.Services.AddScoped<IGrowthMetricService, GrowthMetricService>();
            _ = builder.Services.AddScoped<IFetalGrowthRecordService, FetalGrowthRecordService>();
            _ = builder.Services.AddScoped<IPaymentService, PaymentService>();
            _ = builder.Services.AddScoped<IAccountService, AccountService>();
            _ = builder.Services.AddScoped<IUserMembershipPlanSerivce, UserMembershipPlanService>();
            _ = builder.Services.AddScoped<IGrowthAlertService, GrowthAlertService>();
            _ = builder.Services.AddScoped<IReminderTypeService, ReminderTypeService>();
            _ = builder.Services.AddScoped<IReminderService, ReminderService>();
            _ = builder.Services.AddScoped<IMotherInfoService, MotherInfoService>();
            _ = builder.Services.AddScoped<IStatisticsService, StatisticsService>();
            _ = builder.Services.AddScoped<IFAQCategoryService, FAQCategoryService>();
            _ = builder.Services.AddScoped<IFAQService, FAQService>();
            _ = builder.Services.AddScoped<IFeedBackService, FeedBackService>();
            _ = builder.Services.AddScoped<IReactionService, ReactionService>();
            _ = builder.Services.AddScoped<IContactService, ContactService>();

            _ = builder.Services.AddHttpClient<IChatGPTService, ChatGPTService>();
            _ = builder.Services.AddHttpClient<IChatGeminiService, ChatGeminiService>();

            _ = builder.Services.AddScoped<IReminderNotificationService, ReminderNotificationService>();
            _ = builder.Services.AddHostedService<ReminderBackgroundService>();
            _ = builder.Services.AddScoped<INotificationService, NotificationService>();
            _ = builder.Services.AddScoped<IBlogService, BlogService>();
            _ = builder.Services.AddScoped<ITagService, TagService>();
            _ = builder.Services.AddScoped<ICommentService, CommentService>();
            _ = builder.Services.AddHttpClient<IShoppingService, ShoppingService>();

            _ = builder.Services.AddSignalR();

            // Config identity
            _ = builder.Services.AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>()
                            .AddEntityFrameworkStores<PregnaCareAuthDbContext>()
                            .AddDefaultTokenProviders();

            _ = builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

            // Config CORS
            _ = builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        _ = policy.SetIsOriginAllowed(_ => true)
                              .AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            _ = builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            _ = builder.Services.AddEndpointsApiExplorer();
            _ = builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "PregnaCare API",
                    Version = "v1"
                });

                // Config jwt in Swagger
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter token"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();

            // Seed data
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var appDbContext = services.GetRequiredService<PregnaCareAppDbContext>();
                var authDbContext = services.GetRequiredService<PregnaCareAuthDbContext>();

                authDbContext.Database.Migrate();
                appDbContext.Database.Migrate();

                await SeedData.InitializeAsync(services);
                appDbContext.ChangeTracker.Clear();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                _ = app.UseSwagger();
                _ = app.UseSwaggerUI();
            }

            _ = app.UseHttpsRedirection();

            _ = app.UseCors("AllowFrontend");

            _ = app.MapHub<ReminderHub>("/reminderHub");

            _ = app.UseAuthentication();
            _ = app.UseJwtMiddleware();
            _ = app.UseAuthorization();

            _ = app.MapControllers();

            app.Run();
        }
    }
}