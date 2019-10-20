using DAL.Domains;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly NorthwindContext _context;

        public CategoryRepository(NorthwindContext context)
        {
            _context = context;
        }

        public Category Get(int id)
        {
            return _context.Categories.Find(id);
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Categories.AsNoTracking().ToList();
        }

        public void Update(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
