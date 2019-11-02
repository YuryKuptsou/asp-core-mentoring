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
    public class SupplierService : ISupplierService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Supplier> _supplierRepository;

        public SupplierService(IMapper mapper, IRepository<Supplier> supplierRepository)
        {
            _mapper = mapper;
            _supplierRepository = supplierRepository;
        }

        public IEnumerable<SupplierDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<SupplierDTO>>(_supplierRepository.GetAll());
        }
    }
}
