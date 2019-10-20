using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Domains;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(IMapper mapper, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public CategoryDTO Get(int id)
        {
            return _mapper.Map<CategoryDTO>(_categoryRepository.Get(id));
        }

        public IEnumerable<CategoryDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<CategoryDTO>>(_categoryRepository.GetAll());
        }

        public byte[] GetImage(int id)
        {
            return _categoryRepository.Get(id)?.Picture;
        }

        public void Update(CategoryDTO category)
        {
            _categoryRepository.Update(_mapper.Map<Category>(category));
        }
    }
}
