using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tulip_API.Contracts;
using Tulip_API.Data;
using Tulip_API.DTOs;

namespace Tulip_API.Controllers
{
    /// <summary>
    /// Endpoint used to interact with the Inventories in the Tulip Clothing Store's database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class InventoriesController : ControllerBase
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private string controllerName = "InventoriesController";
        public InventoriesController(IInventoryRepository inventoryRepository,
            ILoggerService logger,
            IMapper mapper)
        {
            _inventoryRepository = inventoryRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get All Inventories
        /// </summary>
        /// <returns>List of Inventory</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetInventories()
        {
            try
            {
                _logger.LogInfo($"{controllerName} - Attempted Get All Inventories");
                var inventories = await _inventoryRepository.FindAll();
                var response = _mapper.Map<IList<InventoryDTO>>(inventories);
                _logger.LogInfo($"{controllerName} - Successfully got all Inventories");
                return Ok(response);
            }
            catch (Exception ex)
            {
                //_logger.LogError($"{ex.Message} - {ex.InnerException}");
                //return StatusCode(500, "Something went wrong. Please contact the administrator"); // 500 something went wrong
                return InternalError($"{controllerName} - {ex.Message} - {ex.InnerException}");
            }
        }
        /// <summary>
        /// Get Inventory by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An Inventory's record</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetInventory(int id)
        {
            try
            {
                _logger.LogInfo($"{controllerName} - Attempted to get Inventory with id:{id}");
                var inventory = await _inventoryRepository.FindById(id);
                if (inventory == null)
                {
                    _logger.LogWarn($"{controllerName} - Inventory with id:{id} was not found");
                    return NotFound(); // 404
                }
                var response = _mapper.Map<InventoryDTO>(inventory);
                _logger.LogInfo($"{controllerName} - Successfully got Inventory with id:{id}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"{controllerName} - {ex.Message} - {ex.InnerException}");
            }
        }
        /// <summary>
        /// Insert new inventory record
        /// </summary>
        /// <param name="inventoryDTO"></param>
        /// <returns>The record that has been inserted</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] InventoryCreateDTO inventoryDTO)
        {
            try
            {
                _logger.LogInfo($"{controllerName} - Inventory submission attempted");
                if (inventoryDTO == null)
                {
                    _logger.LogWarn($"{controllerName} - Empty request was submitted");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid) // Check the validations
                {
                    _logger.LogWarn($"{controllerName} - Inventory Data was incomplete");
                    return BadRequest(ModelState);
                }
                var inventory = _mapper.Map<Inventory>(inventoryDTO);
                var isSuccess = await _inventoryRepository.Create(inventory);
                if (!isSuccess)
                {
                    return InternalError($"{controllerName} - Inventory Creation failed");
                }
                _logger.LogInfo($"{controllerName} - Inventory record successfully inserted");
                return Created("Create", new { inventory });
            }
            catch (Exception ex)
            {

                return InternalError($"{controllerName} - {ex.Message} - {ex.InnerException}");
            }
        }
        private ObjectResult InternalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, "Something went wrong. Please contact the administrator");
        }
    }
}
