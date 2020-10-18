using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tulip_API.Data;
using Tulip_API.DTOs;

namespace Tulip_API.Mappings
{
    public class Maps : Profile
    {
        public Maps()
        {
            CreateMap<Inventory, InventoryDTO>().ReverseMap();
            CreateMap<Inventory, InventoryCreateDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();

        }
    }
}
