using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tulip_API.Contracts;
using Tulip_API.Data;
using Tulip_API.DTOs;

namespace Tulip_API.Controllers
{
    /// <summary>
    /// Endpoint used to interact with the Products in the Tulip Clothing Store's database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository,
            ILoggerService logger, IMapper mapper)
        {
            _productRepository = productRepository;
            _logger = logger;
            _mapper = mapper;
        }
        /// <summary>
        /// Get All Products
        /// </summary>
        /// <returns>List of Products</returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProducts()
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Attempt to get all products");
                var products = await _productRepository.FindAll();
                var response = _mapper.Map<IList<ProductDTO>>(products);
                _logger.LogInfo($"{location}: Got all products successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"{location}: {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Get Product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An Product's record</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProduct(int id)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Attempted to get Product with id:{id}");
                var product = await _productRepository.FindById(id);
                if (product == null)
                {
                    _logger.LogWarn($"{location}: Product with id:{id} was not found");
                    return NotFound(); // 404
                }
                var response = _mapper.Map<ProductDTO>(product);
                _logger.LogInfo($"{location}: Successfully got Product with id:{id}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"{location}: {ex.Message} - {ex.InnerException}");
            }
        }
        /// <summary>
        /// Insert new product record
        /// </summary>
        /// <param name="productDTO"></param>
        /// <returns>The record that has been inserted</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] ProductCreateDTO productDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Product submission attempted");
                if (productDTO == null)
                {
                    _logger.LogWarn($"{location}: Empty request was submitted");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid) // Check the validations
                {
                    _logger.LogWarn($"{location}: Product data was incomplete");
                    return BadRequest(ModelState);
                }
                var product = _mapper.Map<Product>(productDTO);
                var isSuccess = await _productRepository.Create(product);
                if (!isSuccess)
                {
                    return InternalError($"{location}: Product creation failed");
                }
                _logger.LogInfo($"{location}: Product record successfully inserted");
                return Created("Create", new { product });
            }
            catch (Exception ex)
            {
                return InternalError($"{location}: {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Update existing product record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productDTO"></param>
        /// <returns>No content to return</returns>        
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDTO productDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Product with id: {id} updated attempted");
                if (id < 1 || productDTO == null || id != productDTO.Id)
                {
                    _logger.LogWarn($"{location}: Product updating failed (bad data)");
                    return BadRequest();
                }
                var isProductExists = await _productRepository.IsExists(id);
                if (!isProductExists)
                {
                    _logger.LogWarn($"{location}: Category with id: {id} was not found");
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"{location}: Category data was incomplete");
                    return BadRequest(ModelState);
                }
                var product = _mapper.Map<Product>(productDTO);
                var isSuccess = await _productRepository.Update(product);
                if (!isSuccess)
                {
                    return InternalError($"{location}: Updating product failed");
                }
                _logger.LogInfo($"{location}: Product with id: {id} successfully updated");
                return NoContent(); // ok but no content to return
            }
            catch (Exception ex)
            {
                return InternalError($"{location}: {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Delete existing product record
        /// </summary>
        /// <param name="id"></param>
        /// <returns>No content to return</returns>        
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Attempt to delete product with id: {id}");
                if (id < 1)
                {
                    _logger.LogWarn($"{location}: Delete product with id: {id} failed (bad data)");
                    return BadRequest();
                }
                var isProductExists = await _productRepository.IsExists(id);
                if (!isProductExists)
                {
                    _logger.LogWarn($"{location}: Product with id: {id} was not found");
                    return NotFound();
                }
                var product = await _productRepository.FindById(id);
                var isSuccess = await _productRepository.Delete(product);
                if (!isSuccess)
                {
                    return InternalError($"{location}: Deleting product failed");
                }
                _logger.LogInfo($"{location}: Product with id: {id} Successfully deleted");
                return NoContent();
            }
            catch (Exception ex)
            {
                return InternalError($"{location}: {ex.Message} - {ex.InnerException}");
            }
        }
        private string GetControllerActionNames()
        {
            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;

            return $"{controller} - {action}";
        }
        private ObjectResult InternalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, "Something went wrong. Please contact the administrator");
        }
    }
}
