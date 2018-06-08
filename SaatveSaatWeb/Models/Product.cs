using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaatveSaatWeb.Models
{
    public class Product
    {

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public string ProductDesc { get; set; }
        public int ProductStock { get; set; }
        public string ProductImage { get; set; }
        public int ProductBrandCategoryId { get; set; }

        public virtual BrandCategory BrandCategory { get; set; }

    }
}