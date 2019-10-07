using Microsoft.EntityFrameworkCore;
using NorthwindWeb.Infrastructure.Entities;
using NorthwindWeb.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindWeb.Infrastructure.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly NorthwindContext _dbContext;

        public SupplierService(NorthwindContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Supplier> GetAll()
        {
            return _dbContext.Suppliers.AsNoTracking().ToList();
        }

    }
}
