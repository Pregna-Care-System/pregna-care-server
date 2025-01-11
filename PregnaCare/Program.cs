
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PregnaCare.Core.Repositories.Implementations;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.Data;
using PregnaCare.Infrastructure.UnitOfWork;

namespace PregnaCare
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Get connection string
            var authDbConnection = builder.Configuration["ConnectionStrings:AuthDbConnection"];
            var applicationDbConnection = builder.Configuration["ConnectionStrings:ApplicationDbConnection"];
            // Add services to the container.

            builder.Services.AddDbContext<PregnaCareAppDbContext>(options => options.UseSqlServer(applicationDbConnection));
            builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(authDbConnection));

            builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Config identity
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                            .AddEntityFrameworkStores<AuthDbContext>()
                            .AddDefaultTokenProviders();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Migrate, Seed data
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var authDbContext = services.GetRequiredService<AuthDbContext>();

                authDbContext.Database.Migrate();

                await SeedData.InitializeAsync(services);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
