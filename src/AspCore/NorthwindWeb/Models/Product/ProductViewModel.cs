using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindWeb.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [DisplayName("Product name")]
        public string ProductName { get; set; }

        [DisplayName("Quantity per unit")]
        public string QuantityPerUnit { get; set; }

        [DisplayName("Unit price")]
        public decimal UnitPrice { get; set; }

        [DisplayName("Units in stock")]
        public short UnitsInStock { get; set; }

        [DisplayName("Units on order")]
        public short UnitsOnOrder { get; set; }

        [DisplayName("Reorder level")]
        public short ReorderLevel { get; set; }
        public bool Discontinued { get; set; }

        [DisplayName("Supplier")]
        public int SupplierId { get; set; }
        public string CompanyName { get; set; }
        public IEnumerable<SelectListItem> Suppliers { get; set; }

        [DisplayName("Category")]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
