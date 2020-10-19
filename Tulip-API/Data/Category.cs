using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Tulip_API.Data
{
    [Table("Categories")]
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public virtual IList<Product> Products { get; set; }
    }
}
