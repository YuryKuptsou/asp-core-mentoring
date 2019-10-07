using DAL.Domains;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly NorthwindContext _context;

        public SupplierRepository(NorthwindContext context)
        {
            _context = context;
        }

        public IEnumerable<Supplier> GetAll()
        {
            return _context.Suppliers.AsNoTracking().ToList();
        }
    }
}
