
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PregnaCare.Infrastructure.Data;

namespace PregnaCare
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Get connection string
            var authDbConnection = builder.Configuration["ConnectionStrings:AuthDbConnection"];

            // Add services to the container.

            builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(authDbConnection));

            // Config identity
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                            .AddEntityFrameworkStores<AuthDbContext>()
                            .AddDefaultTokenProviders();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var authDbContext = services.GetRequiredService<AuthDbContext>();   
                
                authDbContext.Database.Migrate();
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
