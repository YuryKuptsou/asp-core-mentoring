using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindWeb.Models
{
    public class CategoryViewModel
    {
        public int CategoryID { get; set; }

        [DisplayName("Category")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Please choose file")]
        public IFormFile Image { get; set; }
    }
}
