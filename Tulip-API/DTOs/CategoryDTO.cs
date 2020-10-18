using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Tulip_API.Data;

namespace Tulip_API.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public virtual IList<Product> Products { get; set; }
    }

    public class CategoryCreateDTO
    {
        [Required]
        public string CategoryName { get; set; }
    }
    public class CategoryUpdateDTO
    {
        public int Id { get; set; }
        [Required]
        public string CategoryName { get; set; }
    }
}
