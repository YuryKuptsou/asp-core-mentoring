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
using NorthwindWeb.Infrastructure;
using NorthwindWeb.Infrastructure.Options;


namespace NorthwindWeb
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<ProductOptions>(Configuration);

            services.AddMvcCore()
                .AddRazorViewEngine();

            //configure autofac
            var builder = new ContainerBuilder();
            builder.RegisterModule(new BllAutofacModule(Configuration.GetConnectionString("DefaultConnection")));

            builder.Register(c => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<BllMapperProfile>();
                cfg.AddProfile<NorthwindMapperProfile>();
            })).AsSelf().SingleInstance();
            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>();

            builder.RegisterType<CategoryService>().As<ICategoryService>();
            builder.RegisterType<SupplierService>().As<ISupplierService>();
            builder.RegisterType<ProductService>().As<IProductService>();

            builder.Populate(services);
            var container = builder.Build();

            return new AutofacServiceProvider(container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
