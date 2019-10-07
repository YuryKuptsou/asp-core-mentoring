using DAL.Domains;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly NorthwindContext _context;

        public ProductRepository(NorthwindContext context)
        {
            _context = context;
        }

        public Product Get(int id)
        {
            return _context.Products.Find(id);
        }

        public IEnumerable<Product> GetAll(int count = 0)
        {
            var products = _context.Products.AsNoTracking().ToList();

            if (count == 0)
            {
                return products.ToList();
            }
            else
            {
                return products.Take(count).ToList();
            }
        }

        public int Create(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();

            return product.ProductID;
        }

        public void Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
