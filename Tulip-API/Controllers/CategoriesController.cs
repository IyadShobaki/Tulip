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
    /// Endpoint used to interact with the Categories in the Tulip Clothing Store's database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private string controllerName = "CategoriesController";
        public CategoriesController(ICategoryRepository categoryRepository,
            ILoggerService logger, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _mapper = mapper;
        }
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>List of Category</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                _logger.LogInfo($"{controllerName} - Attempted Get All Categories");
                var categories = await _categoryRepository.FindAll();
                var response = _mapper.Map<IList<CategoryDTO>>(categories);
                _logger.LogInfo($"{controllerName} - Successfully got all Categories");
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
        /// Get Category by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An Category's record</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategory(int id)
        {
            try
            {
                _logger.LogInfo($"{controllerName} - Attempted to get Category with id:{id}");
                var category = await _categoryRepository.FindById(id);
                if (category == null)
                {
                    _logger.LogWarn($"{controllerName} - Category with id:{id} was not found");
                    return NotFound(); // 404
                }
                var response = _mapper.Map<CategoryDTO>(category);
                _logger.LogInfo($"{controllerName} - Successfully got Category with id:{id}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"{controllerName} - {ex.Message} - {ex.InnerException}");
            }
        }
        /// <summary>
        /// Insert new category record
        /// </summary>
        /// <param name="categoryDTO"></param>
        /// <returns>The record that has been inserted</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDTO categoryDTO)
        {
            try
            {
                _logger.LogInfo($"{controllerName} - Category submission attempted");
                if (categoryDTO == null)
                {
                    _logger.LogWarn($"{controllerName} - Empty request was submitted");
                    return BadRequest();
                }
                if (!ModelState.IsValid) // Check the validations
                {
                    _logger.LogWarn($"{controllerName} - Category data was incomplete");
                    return BadRequest(ModelState);
                }
                var category = _mapper.Map<Category>(categoryDTO);
                var isSuccess = await _categoryRepository.Create(category);
                if (!isSuccess)
                {
                    return InternalError($"{controllerName} - Category creation failed");
                }
                _logger.LogInfo($"{controllerName} - Category record successfully inserted");
                return Created("Create", new { category });
            }
            catch (Exception ex)
            {
                return InternalError($"{controllerName} - {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Update existing category record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categoryDTO"></param>
        /// <returns>No content to return</returns>        
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateDTO categoryDTO)
        {
            try
            {
                _logger.LogInfo($"{controllerName} - Category with id: {id} updated attempted");
                if (id < 1 || categoryDTO == null || id != categoryDTO.Id)
                {
                    _logger.LogWarn($"{controllerName} - Category updating failed (bad data)");
                    return BadRequest();
                }
                var isCategoryExists = await _categoryRepository.IsExists(id);
                if (!isCategoryExists)
                {
                    _logger.LogWarn($"{controllerName} - Category with id: {id} was not found");
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"{controllerName} - Category data was incomplete");
                    return BadRequest(ModelState);
                }
                var category = _mapper.Map<Category>(categoryDTO);
                var isSuccess = await _categoryRepository.Update(category);
                if (!isSuccess)
                {
                    return InternalError($"{controllerName} - Updating category failed");
                }
                _logger.LogWarn($"{controllerName} - Category with id: {id} successfully updated");
                return NoContent(); // ok but no content to return
            }
            catch (Exception ex)
            {
                return InternalError($"{controllerName} - {ex.Message} - {ex.InnerException}");
            }
        }


        /// <summary>
        /// Delete existing category record
        /// </summary>
        /// <param name="id"></param>
        /// <returns>No content to return</returns>        
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInfo($"{controllerName} - Attempt to delete category with id: {id}");
                if (id < 1)
                {
                    _logger.LogWarn($"{controllerName} - Delete category with id: {id} failed (bad data)");
                    return BadRequest();
                }
                var isCategoryExists = await _categoryRepository.IsExists(id);
                if (!isCategoryExists)
                {
                    _logger.LogWarn($"{controllerName} - Category with id: {id} was not found");
                    return NotFound();
                }
                var category = await _categoryRepository.FindById(id);  
                var isSuccess = await _categoryRepository.Delete(category);
                if (!isSuccess)
                {
                    return InternalError($"{controllerName} - Deleting category failed");
                }
                _logger.LogWarn($"{controllerName} - Category with id: {id} Successfully deleted");
                return NoContent();
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
