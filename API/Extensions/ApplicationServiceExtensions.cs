using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using API.SignalR;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // Add the JWT token service and other repositories
            services.AddSingleton<PresenceTracker>(); // dictionary containing all the online users
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings")); // point the program from where to take the configuration
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoService, PhotoService>(); // service to communicate with cloudinary
            services.AddScoped<IUnitOfWork, UnitOfWork>(); // this has all of our services (users, messages and likes)
            services.AddScoped<LogUserActivity>(); // service to update lastActive property of logged user
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            // Add the connection to the DB
            services.AddDbContext<DataContext>(options => 
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}