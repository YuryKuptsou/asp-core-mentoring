using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Domains
{
    public class Category : IEntity
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }
        public string ContentType { get; set; }
    }
}
