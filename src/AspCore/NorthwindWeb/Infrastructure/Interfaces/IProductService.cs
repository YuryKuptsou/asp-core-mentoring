﻿using NorthwindWeb.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindWeb.Infrastructure.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll(int count);
        Product Get(int id);
        int Create(Product product);
        void Update(Product product);
    }
}
