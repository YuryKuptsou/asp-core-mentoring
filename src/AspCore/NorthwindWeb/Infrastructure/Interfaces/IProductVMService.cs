using NorthwindWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindWeb.Infrastructure.Interfaces
{
    public interface IProductVMService
    {
        void PopulateSelectLists(ProductViewModel model);
    }
}
