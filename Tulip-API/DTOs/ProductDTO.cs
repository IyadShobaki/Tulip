using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tulip_API.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string ProductImage { get; set; }
        public decimal RetailPrice { get; set; }
        public int QuantityInStock { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsTaxable { get; set; }
        public bool? Sex { get; set; } // ? to allow nulls

        // Allow for us to know this product follow which inventory record
        public virtual InventoryDTO Inventory { get; set; }
    }
}
