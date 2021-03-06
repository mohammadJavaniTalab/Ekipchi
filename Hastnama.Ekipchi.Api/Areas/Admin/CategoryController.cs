﻿using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Ekipchi.Business.Service;
using Hastnama.Ekipchi.Common.Helper;
using Hastnama.Ekipchi.Common.Message;
using Hastnama.Ekipchi.Data.Category;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Ekipchi.Api.Areas.Admin
{
    public class CategoryController : BaseAdminController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// Category List
        /// </summary>
        /// <param name="filterQueryDto"></param>
        /// <returns>Category List</returns>
        /// <response code="200">if Get successfully </response>
        /// <response code="404">If entity not found.</response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(PagedList<CategoryDto>), 200)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpGet]
        public async Task<IActionResult> List(
            [FromQuery] FilterCategoryQueryDto filterQueryDto)
        {
            var result = await _unitOfWork.CategoryService.List(filterQueryDto);
            if (filterQueryDto.Page == null && filterQueryDto.Limit == null)
                return Ok(result.Data.Items);
            return result.ApiResult;
        }

        /// <summary>
        /// Category Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Category Detail</returns>
        /// <response code="200">if Get successfully </response>
        /// <response code="404">If entity not found.</response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(CategoryDto), 200)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpGet("{id}", Name = "GetCategory")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _unitOfWork.CategoryService.Get(id);
            return result.ApiResult;
        }

        /// <summary>
        /// Update Category 
        /// </summary>
        /// <param name="updateCategoryDto"></param>
        /// <param name="id"></param>
        /// <returns>NoContent</returns>
        /// <response code="204">if Update successfully </response>
        /// <response code="400">If validation failure.</response>
        /// <response code="404">If entity not found.</response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDto updateCategoryDto)
        {
            updateCategoryDto.Id = id;
            var result = await _unitOfWork.CategoryService.Update(updateCategoryDto);

            if (!result.Success)
                return result.ApiResult;
            return NoContent();
        }


        /// <summary>
        /// Create Category 
        /// </summary>
        /// <param name="createCategoryDto"></param>
        /// <returns>NoContent</returns>
        /// <response code="201">if Create successfully </response>
        /// <response code="400">If validation failure.</response>
        /// <response code="404">If entity not found.</response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto createCategoryDto)
        {
            var result = await _unitOfWork.CategoryService.Create(createCategoryDto);
            if (!result.Success)
                return result.ApiResult;
            return Created(Url.Link("GetCounty", new {result.Data.Id}), _mapper.Map<CategoryDto>(result.Data));
        }

        /// <summary>
        /// Delete Category 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>NoContent</returns>
        /// <response code="204">if Delete successfully </response>
        /// <response code="404">If entity not found.</response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _unitOfWork.CategoryService.Delete(id);
            if (!result.Success)
                return result.ApiResult;
            return NoContent();
        }
    }
}