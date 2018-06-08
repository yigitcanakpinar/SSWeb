using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SaatveSaatWeb.Models
{
    public class BrandCategory
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BrandCategory()
        {
            this.Product = new HashSet<Product>();
        }

        public int BrandCategoryId { get; set; }
        public int BrandCategoryBrandId { get; set; }
        public int BrandCategoryCategoryId { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Category Category { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Product { get; set; }
    }
}