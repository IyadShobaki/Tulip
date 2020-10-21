using System;

namespace Tulip_BlazorUI.Models
{
    public class Product
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
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}