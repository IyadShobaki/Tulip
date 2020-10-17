using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tulip_API.Contracts;

namespace Tulip_API.Controllers
{
    /// <summary>
    /// Endpoint used to interact with the Inventories in the Tulip Clothing Store's database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    public class InventoriesController : ControllerBase
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoriesController(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }
    }
}
