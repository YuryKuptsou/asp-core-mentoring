﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Domains
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }
    }
}
