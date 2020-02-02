﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hastnama.Ekipchi.Business.Service.Interface;
using Hastnama.Ekipchi.Common.Enum;
using Hastnama.Ekipchi.Common.General;
using Hastnama.Ekipchi.Common.Message;
using Hastnama.Ekipchi.Data.Auth;
using Hastnama.Ekipchi.Data.User;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Ekipchi.Api.Areas.Admin
{
    public class UserController : BaseAdminController
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// User List
        /// </summary>
        /// <param name="filterQueryDto"></param>
        /// <param name="pagingOptions"></param>
        /// <returns>User List</returns>
        /// <response code="200">if login successfully </response>
        /// <response code="400">If validation failure.</response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(List<UserDto>), 200)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] PagingOptions pagingOptions,
            [FromQuery] UserFilterQueryDto filterQueryDto)
        {
            var result = await _unitOfWork.UserService.List(pagingOptions, filterQueryDto);
            return result.ApiResult;
        }

        /// <summary>
        /// User Profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns>User Profile</returns>
        /// <response code="200">if login successfully </response>
        /// <response code="400">If validation failure.</response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
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
        /// <param name="updateUserDto"></param>
        /// <returns>NoContent</returns>
        /// <response code="200">if login successfully </response>
        /// <response code="400">If validation failure.</response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] UpdateUserDto updateUserDto)
        {
            var result = await _unitOfWork.UserService.UpdateProfile(updateUserDto);
            if (!result.Success)
                return result.ApiResult;
            return NoContent();
        }

        /// <summary>
        /// Delete User 
        /// </summary>
        /// <param name="updateUserDto"></param>
        /// <returns>NoContent</returns>
        /// <response code="200">if login successfully </response>
        /// <response code="400">If validation failure.</response>
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
        /// <response code="200">if login successfully </response>
        /// <response code="400">If validation failure.</response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] CreateUserDto createUserDto)
        {
            var result = await _unitOfWork.UserService.Create(createUserDto);
            if (!result.Success)
                return result.ApiResult;
            return NoContent();
        }
    }
}