﻿using System;
using AutoMapper;
using Hastnama.Ekipchi.Common.Message;
using Hastnama.Ekipchi.Data.Event;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hastnama.Ekipchi.Business.Service;
using Hastnama.Ekipchi.Data.Event.Gallery;

namespace Hastnama.Ekipchi.Api.Areas.Admin
{
    public class EventController : BaseAdminController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Event List
        /// </summary>
        /// <param name="filterQueryDto"></param>
        /// <returns>Event List</returns>
        /// <response code="200">if everything is ok </response>
        /// <response code="400">If validation failure.</response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(List<EventDto>), 200)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] FilterEventQueryDto filterQueryDto)
        {
            var result = await _unitOfWork.EventService.List(filterQueryDto);
            if (filterQueryDto.Page == null && filterQueryDto.Limit == null)
                return Ok(result.Data.Items);
            return result.ApiResult;
        }

        /// <summary>
        /// Event
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Event</returns>
        /// <response code="200">if item found </response>
        /// <response code="400">If validation failure.</response>
        /// <response code="404">If item not found.</response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(EventDto), 200)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpGet("{id}", Name = "GetEvent")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var events = await _unitOfWork.EventService.Get(id);
            return events.ApiResult;
        }

        /// <summary>
        /// Create Event 
        /// </summary>
        /// <param name="createEvent"></param>
        /// <returns>NoContent</returns>
        /// <response code="201">if item created </response>
        /// <response code="400">If validation failure.</response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpPost]
        public async Task<IActionResult> Post(CreateEventDto createEvent)
        {
            var result = await _unitOfWork.EventService.Create(createEvent, UserId);
            return Created(Url.Link("GetEvent", new {result.Data.Id}), _mapper.Map<EventDto>(result.Data));
        }

        /// <summary>
        /// Update Event 
        /// </summary>
        /// <param name="updateEvent"></param>
        /// <param name="id"></param>
        /// <returns>NoContent</returns>
        /// <response code="204">if updated successfully </response>
        /// <response code="400">If validation failure.</response>
        /// <response code="404">If item not found.</response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateEventDto updateEvent)
        {
            updateEvent.Id = id;
            var result = await _unitOfWork.EventService.Update(updateEvent, UserId);

            if (!result.Success)
                return result.ApiResult;

            return NoContent();
        }

        /// <summary>
        /// Update Event 
        /// </summary>
        /// <param name="UpdateEventGalleryDto"></param>
        /// <param name="id"></param>
        /// <returns>NoContent</returns>
        /// <response code="204">if updated successfully </response>
        /// <response code="400">If validation failure.</response>
        /// <response code="404">If item not found.</response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpPut("{id}/gallery")]
        public async Task<IActionResult> UpdateGallery(Guid id, UpdateEventGalleryDto UpdateEventGalleryDto)
        {
            UpdateEventGalleryDto.Id = id;
            var result = await _unitOfWork.EventService.UpdateGallery(UpdateEventGalleryDto);

            if (!result.Success)
                return result.ApiResult;

            return NoContent();
        }

        /// <summary>
        /// Delete Event
        /// </summary>
        /// <param name="id"></param>
        /// <returns>NoContent</returns>
        /// <response code="204">if item deleted </response>
        /// <response code="400">If validation failure.</response>
        /// <response code="404">If item not found.</response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _unitOfWork.EventService.Delete(id);

            if (!result.Success)
                return result.ApiResult;

            return NoContent();
        }
    }
}