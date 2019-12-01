using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
    public interface IProductService
    {
        IEnumerable<ProductDTO> GetAll(int count = 0);
        ProductDTO Get(int id);
        int CreateOrUpdate(ProductDTO product);
        void Remove(ProductDTO product);
    }
}
