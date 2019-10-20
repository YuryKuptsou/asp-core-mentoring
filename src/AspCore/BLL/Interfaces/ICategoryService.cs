using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<CategoryDTO> GetAll();
        CategoryDTO Get(int id);
        byte[] GetImage(int id);

        void Update(CategoryDTO category);
    }
}
