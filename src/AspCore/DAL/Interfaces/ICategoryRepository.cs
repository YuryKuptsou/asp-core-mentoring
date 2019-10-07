using DAL.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Interfaces
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAll();
    }
}
