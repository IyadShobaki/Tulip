using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tulip_API.Data
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public virtual IList<Product> Products { get; set; }
    }
}
