using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using BLL.Infrastructure;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NorthwindWeb.Filters;
using NorthwindWeb.Infrastructure;
using NorthwindWeb.Infrastructure.Extensions;
using NorthwindWeb.Infrastructure.Interfaces;
using NorthwindWeb.Infrastructure.Options;
using NorthwindWeb.Infrastructure.Services;

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

            services.AddMvcCore(options => options.Filters.Add(typeof(LogActionFilter)))
                .AddRazorViewEngine();

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

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IOptions<ImageCacheOptions> options)
        {
            var imageCache = options.Value;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseImageCache(imageCache.ImageCacheFolder, imageCache.MaxCachedImages, imageCache.ImageCacheExpiration);

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute("image", "images/{id}", new { Controller = "Category", Action = "Image" });
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
