﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Ekipchi.Business.Service;
using Hastnama.Ekipchi.Common.Enum;
using Hastnama.Ekipchi.Common.Helper;
using Hastnama.Ekipchi.Common.Message;
using Hastnama.Ekipchi.Data.Event;
using Hastnama.Ekipchi.Data.Group;
using Hastnama.Ekipchi.Data.User;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Ekipchi.Api.Areas.Admin
{
    public class UserController : BaseAdminController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// User List
        /// </summary>
        /// <param name="filterQueryDto"></param>
        /// <returns>User List</returns>
        /// <response code="200">if Get List successfully </response>
        /// <response code="404">If entity not found.</response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(PagedList<UserDto>), 200)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] FilterUserQueryDto filterQueryDto)
        {
            var result = await _unitOfWork.UserService.List(filterQueryDto);
            if (filterQueryDto.Page == null && filterQueryDto.Limit == null)
                return Ok(result.Data.Items);
            return result.ApiResult;
        }

        /// <summary>
        /// User Profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns>User Profile</returns>
        /// <response code="200">if Get successfully </response>
        /// <response code="404">If entity not found.</response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _unitOfWork.UserService.Get(id);

            return result.ApiResult;
        }

        /// <summary>
        /// Update User 
        /// </summary>
        /// <param name="adminUpdateUserDto"></param>
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
        public async Task<IActionResult> Update(Guid id, [FromBody] AdminUpdateUserDto adminUpdateUserDto)
        {
            adminUpdateUserDto.Id = id;
            var result = await _unitOfWork.UserService.UpdateProfile(adminUpdateUserDto);

            if (!result.Success)
                return result.ApiResult;
            return NoContent();
        }

        /// <summary>
        /// Update User 
        /// </summary>
        /// <param name="adminUpdateUserStatusDto"></param>
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
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] AdminUpdateUserStatusDto adminUpdateUserStatusDto)
        {
            var result = await _unitOfWork.UserService.UpdateStatus(id,adminUpdateUserStatusDto.Status);

            if (!result.Success)
                return result.ApiResult;
            return NoContent();
        }

        /// <summary>
        /// Delete User 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>NoContent</returns>
        /// <response code="204">if Delete successfully </response>
        /// <response code="404">If entity not found.</response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _unitOfWork.UserService.UpdateStatus(id, UserStatus.Delete);
            if (!result.Success)
                return result.ApiResult;
            return NoContent();
        }

        /// <summary>
        /// Create User 
        /// </summary>
        /// <param name="createUserDto"></param>
        /// <returns>NoContent</returns>
        /// <response code="201">if Create successfully </response>
        /// <response code="400">If validation failure.</response>
        /// <response code="404">If entity not found.</response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto createUserDto)
        {
            var result = await _unitOfWork.UserService.Create(createUserDto);
            return !result.Success ? result.ApiResult : Created(Url.Link("GetUser", new {result.Data.Id}), _mapper.Map<UserDto>(result.Data));
        }


        /// <summary>
        /// User Groups
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Group List</returns>
        /// <response code="200">if Get List successfully </response>
        /// <response code="404">If entity not found.</response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(List<GroupDto>), 200)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpGet("{id}/Groups")]
        public async Task<IActionResult> UserGroups(Guid id)
        {
            var result = await _unitOfWork.UserService.UserGroups(id);
            return result.ApiResult;
        }

        /// <summary>
        /// User Events
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Group List</returns>
        /// <response code="200">if Get List successfully </response>
        /// <response code="404">If entity not found.</response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(List<EventDto>), 200)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpGet("{id}/Events")]
        public async Task<IActionResult> UserEvents(Guid id)
        {
            var result = await _unitOfWork.UserService.UserEvents(id);
            return result.ApiResult;
        }
    }
}