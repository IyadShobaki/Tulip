using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tulip_API.Data
{
    [Table("Products")]
    public partial class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string ProductImage { get; set; }
        [Column(TypeName = "decimal(18,2)")] // fix for Microsoft.EntityFrameworkCore.Model.Validation[30000]
        public decimal RetailPrice { get; set; }
        public int QuantityInStock { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsTaxable { get; set; }
        public int CategoryId { get; set; }

        // Allow for us to know this product follow which category
        public virtual Category Category { get; set; }
    }
}