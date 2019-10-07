using DAL.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Interfaces
{

    public interface IProductRepository
    {
        IEnumerable<Product> GetAll(int count = 0);
        Product Get(int id);
        int Create(Product product);
        void Update(Product product);
    }
}
