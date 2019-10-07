using DAL.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Interfaces
{
    public interface ISupplierCategory
    {
        IEnumerable<Supplier> GetAll();
    }
}
