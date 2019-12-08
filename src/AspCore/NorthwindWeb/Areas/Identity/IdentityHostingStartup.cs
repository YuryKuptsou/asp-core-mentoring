﻿using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NorthwindWeb.Infrastructure.Services;
using NorthwindWeb.Models;

[assembly: HostingStartup(typeof(NorthwindWeb.Areas.Identity.IdentityHostingStartup))]
namespace NorthwindWeb.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<NorthwindContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("NorthwindContextConnection")));

                services.AddDefaultIdentity<IdentityUser>()
                    .AddEntityFrameworkStores<NorthwindContext>();
                
            });
        }
    }
}