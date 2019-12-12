using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using BLL.Infrastructure;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NorthwindWeb.Filters;
using NorthwindWeb.Infrastructure;
using NorthwindWeb.Infrastructure.Extensions;
using NorthwindWeb.Infrastructure.Interfaces;
using NorthwindWeb.Infrastructure.Options;
using NorthwindWeb.Infrastructure.Services;
using NorthwindWeb.Models;
using SmartBreadcrumbs.Extensions;
using Swashbuckle.AspNetCore.Swagger;

namespace NorthwindWeb
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;


        public IConfiguration Configuration { get; set; }

        

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var connstring = Configuration.GetConnectionString("DefaultConnection");
            _logger.LogInformation("Read default connection string: {connString}", connstring);

            services.AddOptions();
            services.Configure<ProductOptions>(Configuration);
            services.Configure<ImageCacheOptions>(Configuration);
            services.Configure<LogActionOptions>(Configuration);
            services.Configure<AuthMessageSenderOptions>(Configuration);



            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<NorthwindContext>();

            services.AddAuthentication()
                .AddAzureAD(options => Configuration.Bind("AzureAd", options))
                .AddCookie(); ;

            services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, options =>
            {
                options.Authority = options.Authority + "/v2.0/";
                options.TokenValidationParameters.ValidateIssuer = false;
            });


            services.AddMvcCore(options => options.Filters.Add(typeof(LogActionFilter)))
                .AddRazorViewEngine()
                .AddApiExplorer()
                .AddJsonFormatters()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Formatting = Formatting.Indented;
                });

            services.AddBreadcrumbs(GetType().Assembly);

            services.AddTransient<IEmailSender, EmailSender>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Northwind API doc", Version = "v1" });
                var filePath = Path.Combine(AppContext.BaseDirectory, "MyApi.xml");
                c.IncludeXmlComments(filePath);
            });

            //configure autofac
            var builder = new ContainerBuilder();
            builder.RegisterModule(new BllAutofacModule(connstring));

            builder.Register(c => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<BllMapperProfile>();
                cfg.AddProfile<NorthwindMapperProfile>();
            })).AsSelf().SingleInstance();
            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>();

            builder.RegisterType<CategoryService>().As<ICategoryService>();
            builder.RegisterType<SupplierService>().As<ISupplierService>();
            builder.RegisterType<ProductService>().As<IProductService>();
            builder.RegisterType<ProductVMService>().As<IProductVMService>();

            builder.Populate(services);
            var container = builder.Build();

            return new AutofacServiceProvider(container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IOptions<ImageCacheOptions> imageCacheOptions, IServiceProvider serviceProvider)
        {
            var imageCache = imageCacheOptions.Value;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            //app.UseImageCache(imageCache.ImageCacheFolder, imageCache.MaxCachedImages, imageCache.ImageCacheExpiration);
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute("image", "images/{id}", new { Controller = "Category", Action = "Image" });
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "swagger";
                options.DocumentTitle = "Northwind API documentation";
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Northwind API V1");
            });

            CreateAdminUser(serviceProvider).Wait();
        }

        private async Task CreateAdminUser(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var role = "Administrator";

            var roleExist = await roleManager.RoleExistsAsync(role);

            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }

            var email = Configuration["UserEmail"];
            var pass = Configuration["UserPass"];

            var admin = new IdentityUser
            {

                UserName = email,
                Email = email,
            };

            var _user = await userManager.FindByEmailAsync(email);

            if (_user == null)
            {
                var result = await userManager.CreateAsync(admin, pass);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, role);

                }
            }
        }
    }
}
