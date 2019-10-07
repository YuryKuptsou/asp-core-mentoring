using Autofac;
using DAL;
using DAL.Interfaces;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Infrastructure
{
    public class BllAutofacModule : Module
    {
        private readonly string _connectionString;

        public BllAutofacModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NorthwindContext>().WithParameter("connString", _connectionString);

            builder.RegisterType<SupplierRepository>().As<ISupplierRepository>();
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>();
            builder.RegisterType<ProductRepository>().As<IProductRepository>();
            
        }
    }
}
