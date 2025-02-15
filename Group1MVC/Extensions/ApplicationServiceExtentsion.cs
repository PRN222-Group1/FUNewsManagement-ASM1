using BusinessServiceLayer.Interfaces;
using BusinessServiceLayer.Services;
using Group1MVC.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using RepositoryLayer.Interfaces;

namespace Group1MVC.Extensions
{
    public static class ApplicationServiceExtentsion
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {
            // Registers the database context with the DI container
            services.AddDbContext<FuNewsManagementContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register AutoMapper
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            // Register Services
            services.AddScoped<IAccountService, AccountService>();

            // Configure Cookie
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Add Authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            return services;
        }
    }
}
