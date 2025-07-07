using System;
using System.Collections.Generic;
using BierAlyzer.Api.Services;
using BierAlyzer.Contracts.Communication.Event;
using BierAlyzer.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BierAlyzer.Api.Controllers
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Manage events. </summary>
    /// <remarks>   Andre Beging, 18.06.2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    [Route("api/event")]
    public class EventController : Controller
    {
        private readonly EventService _eventService;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Events controller. </summary>
        /// <remarks>   Andre Beging, 18.06.2018. </remarks>
        /// <param name="eventService"> The service. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public EventController(EventService eventService)
        {
            _eventService = eventService;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get all events. </summary>
        /// <remarks>   Andre Beging, 20.06.2018. </remarks>
        /// <returns>   Result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet]
        [SwaggerResponse(200, Type = typeof(EventResponse))]
        [SwaggerResponse(400, Type = typeof(EventResponse))]
        public IActionResult Get()
        {
            var dtoEvents = _eventService.GetEvents(HttpContext.User.Claims);
            return Ok(dtoEvents);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get a specific event. </summary>
        /// <remarks>   Andre Beging, 18.11.2018. </remarks>
        /// <param name="eventId">  The event id to get. </param>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet("{eventid}")]
        [SwaggerResponse(200, Type = typeof(EventResponse))]
        [SwaggerResponse(400, Type = typeof(EventResponse))]
        public IActionResult Get(Guid eventId)
        {
            var response = _eventService.GetEvent(eventId, HttpContext.User.Claims);

            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }
    }
}