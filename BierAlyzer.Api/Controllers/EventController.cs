using System;
using System.Collections.Generic;
using BierAlyzer.Api.Services;
using BierAlyzer.Contracts.Communication.Event;
using BierAlyzer.Contracts.Communication.Event.Request;
using BierAlyzer.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

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
        [SwaggerResponse(200, typeof(EventResponse))]
        [SwaggerResponse(400, typeof(EventResponse))]
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
        [SwaggerResponse(200, typeof(EventResponse))]
        [SwaggerResponse(400, typeof(EventResponse))]
        public IActionResult Get(Guid eventId)
        {
            var response = _eventService.GetEvent(eventId, HttpContext.User.Claims);

            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpPost("join")]
        [SwaggerResponse(200, typeof(EventResponse))]
        [SwaggerResponse(400, typeof(EventResponse))]
        public IActionResult Join([FromBody] JoinEventRequest request)
        {
            var response = _eventService.JoinEvent(request, HttpContext.User.Claims);
            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpPost("join/{id}")]
        [SwaggerResponse(200, typeof(EventResponse))]
        [SwaggerResponse(400, typeof(EventResponse))]
        public IActionResult JoinPublic(Guid id)
        {
            var response = _eventService.JoinPublicEvent(id, HttpContext.User.Claims);
            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpPost("leave/{id}")]
        [SwaggerResponse(200, typeof(EventResponse))]
        [SwaggerResponse(400, typeof(EventResponse))]
        public IActionResult Leave(Guid id)
        {
            var response = _eventService.LeaveEvent(id, HttpContext.User.Claims);
            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpPost]
        [SwaggerResponse(200, typeof(EventResponse))]
        [SwaggerResponse(400, typeof(EventResponse))]
        public IActionResult Create([FromBody] CreateEventRequest request)
        {
            var response = _eventService.CreateEvent(request, HttpContext.User.Claims);
            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpPut]
        [SwaggerResponse(200, typeof(EventResponse))]
        [SwaggerResponse(400, typeof(EventResponse))]
        public IActionResult Update([FromBody] UpdateEventRequest request)
        {
            var response = _eventService.UpdateEvent(request, HttpContext.User.Claims);
            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpDelete("{id}")]
        [SwaggerResponse(200, typeof(EventResponse))]
        [SwaggerResponse(400, typeof(EventResponse))]
        public IActionResult Remove(Guid id)
        {
            var response = _eventService.RemoveEvent(id, HttpContext.User.Claims);
            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpPost("status")]
        [SwaggerResponse(200, typeof(EventResponse))]
        [SwaggerResponse(400, typeof(EventResponse))]
        public IActionResult SetStatus([FromBody] SetEventStatusRequest request)
        {
            var response = _eventService.SetStatus(request, HttpContext.User.Claims);
            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpPost("book")]
        [SwaggerResponse(200, typeof(EventResponse))]
        [SwaggerResponse(400, typeof(EventResponse))]
        public IActionResult BookDrink([FromBody] BookDrinkRequest request)
        {
            var response = _eventService.BookDrink(request, HttpContext.User.Claims);
            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }
    }
}