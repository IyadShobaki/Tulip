using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int CategoryId { get; set; }

        public virtual CategoryDTO Category { get; set; }
    }
    public class ProductCreateDTO
    {
        [Required]
        public string ProductName { get; set; }
        [Required]
        [StringLength(4000)]
        public string Description { get; set; }
        [Required]
        public string ProductImage { get; set; }
        [Required]
        public decimal RetailPrice { get; set; }
        [Required]
        public int QuantityInStock { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public DateTime UpdatedDate { get; set; }
        [Required]
        public bool IsTaxable { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
    public class ProductUpdateDTO
    {
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        [StringLength(4000)]
        public string Description { get; set; }
        [Required]
        public string ProductImage { get; set; }
        [Required]
        public decimal RetailPrice { get; set; }
        [Required]
        public int QuantityInStock { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public DateTime UpdatedDate { get; set; }
        [Required]
        public bool IsTaxable { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
