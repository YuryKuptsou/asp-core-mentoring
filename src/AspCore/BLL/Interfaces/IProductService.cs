using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
    public interface IProductService
    {
        IEnumerable<ProductDTO> GetAll(int count);
        ProductDTO Get(int id);
        void CreateOrUpdate(ProductDTO product);
    }
}
