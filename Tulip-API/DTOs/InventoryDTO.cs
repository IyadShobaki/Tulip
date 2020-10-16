using System;
using System.Collections.Generic;
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
}
