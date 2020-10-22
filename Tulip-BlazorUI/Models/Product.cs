using System;
using System.ComponentModel.DataAnnotations;

namespace Tulip_BlazorUI.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string Description { get; set; }

        public string ProductImage { get; set; }
        [Required]
        //[RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(1, 9999999999999999.99)]
        public decimal RetailPrice { get; set; }
        [Required]
        [Range(1, 9999999999999999)]
        public int QuantityInStock { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public DateTime UpdatedDate { get; set; }
        [Required]
        public bool IsTaxable { get; set; }
        [Required]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}