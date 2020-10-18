using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tulip_API.DTOs
{
    public class InventoryDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public virtual IList<ProductDTO> Products { get; set; }
    }
    public class InventoryCreateDTO
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal PurchasePrice { get; set; }
        [Required]
        public DateTime PurchaseDate { get; set; }
    }
}
