using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindWeb.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        [DisplayName("Product name")]
        public string ProductName { get; set; }

        [StringLength(20)]
        [DisplayName("Quantity per unit")]
        public string QuantityPerUnit { get; set; }

        [Range(0.01, 10000)]
        [DisplayName("Unit price")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}")]
        public decimal UnitPrice { get; set; }

        [Range(0, 10000)]
        [DisplayName("Units in stock")]
        public short UnitsInStock { get; set; }

        [Range(0, 10000)]
        [DisplayName("Units on order")]
        public short UnitsOnOrder { get; set; }

        [Range(0, 10000)]
        [DisplayName("Reorder level")]
        public short ReorderLevel { get; set; }


        public bool Discontinued { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "Choose supplier")]
        [DisplayName("Supplier")]
        public int SupplierId { get; set; }
        public string CompanyName { get; set; }
        public IEnumerable<SelectListItem> Suppliers { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Choose category")]
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
