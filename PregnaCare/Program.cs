
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PregnaCare.Core.Repositories.Implementations;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.Data;
using PregnaCare.Infrastructure.UnitOfWork;
using PregnaCare.Middlewares;
using PregnaCare.Services.Implementations;
using PregnaCare.Services.Interfaces;

namespace PregnaCare
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Env.Load();

            var builder = WebApplication.CreateBuilder(args);

            // Get connection string
            var authDbConnection = builder.Configuration["ConnectionStrings:AuthDbConnection"];
            var applicationDbConnection = builder.Configuration["ConnectionStrings:ApplicationDbConnection"];

            // Add services to the container.
            builder.Services.AddDbContext<PregnaCareAppDbContext>(options => options.UseSqlServer(applicationDbConnection));

            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            // Config identity
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                            .AddEntityFrameworkStores<PregnaCareAppDbContext>()
                            .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

            // Config CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
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
                var appDbcContext = services.GetRequiredService<PregnaCareAppDbContext>();

                appDbcContext.Database.Migrate();

                await SeedData.InitializeAsync(services);
                appDbcContext.ChangeTracker.Clear();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowFrontend");

            app.UseAuthentication();
            app.UseJwtMiddleware();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
