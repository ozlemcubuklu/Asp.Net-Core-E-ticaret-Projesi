﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Entity
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public List<ProductCategory> ProductCategory { get; set; }
    }
}
