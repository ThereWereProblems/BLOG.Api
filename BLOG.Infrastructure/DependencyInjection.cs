﻿using BLOG.Application.Common.Abstractions;
using BLOG.Domain.Model.ApplicationUser;
using BLOG.Infrastructure.Persistance;
using BLOG.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(ICommunicationServiceClient), typeof(CommunicationServiceClient));
            services.AddScoped(typeof(ICurentUserService), typeof(CurentUserService));
            services.AddScoped(typeof(IApplicationDbContext), typeof(ApplicationDbContext));

            //lazy loading
            services.AddScoped<Lazy<ICurentUserService>>(provider => new Lazy<ICurentUserService>(() => provider.GetRequiredService<ICurentUserService>()));

            services.AddDbContextFactory<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DbCennection"));
            });

            return services;
        }
    }
}
