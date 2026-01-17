using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using BierAlyzer.Api.Helper;
using BierAlyzer.Api.Models;
using BierAlyzer.Contracts.Communication.Event;
using BierAlyzer.Contracts.Communication.Event.Request;
using BierAlyzer.Contracts.Dto;
using BierAlyzer.Contracts.Model;
using BierAlyzer.EntityModel;

namespace BierAlyzer.Api.Services
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An event service. </summary>
    /// <remarks>   Andre Beging, 10.11.2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class EventService : BierAlyzerServiceBase
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        /// <remarks>   Andre Beging, 18.11.2018. </remarks>
        /// <param name="context">  The context. </param>
        /// <param name="mapper">   The mapper. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public EventService(BierAlyzerContext context, IMapper mapper) : base(context, mapper)
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the events. </summary>
        /// <remarks>   Andre Beging, 18.11.2018. </remarks>
        /// <returns>   The events. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public EventResponse GetEvents(IEnumerable<Claim> claims)
        {
            try
            {
                var response = new EventResponse();

                if (!claims.TryGetValue<Guid>(BierAlyzerClaim.UserId, out var userId))
                    response.Result.Status = RequestResultStatus.TokenError;

                if (!claims.TryGetValue<UserType>(BierAlyzerClaim.UserType, out var userType))
                    response.Result.Status = RequestResultStatus.TokenError;

                // Does anything went wrong?
                if (!response.Result.Success) return response;

                var contextEvents = Context.Event.Where(e => e.EventId != Guid.Empty);
                if (userType == UserType.User)
                {
                    // A user has to be the owner of or registered to the event
                    contextEvents = contextEvents.Where(e => e.OwnerId == userId || e.EventUsers.Any(eu => eu.UserId == userId));
                }

                response.Events.AddRange(Mapper.Map<List<EventDto>>(contextEvents));

                return response;
            }
            catch (Exception e)
            {
                return new EventResponse
                {
                    Result = new RequestResult(RequestResultStatus.ServerError, e.Message)
                };
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets an event. </summary>
        /// <remarks>   Andre Beging, 18.11.2018. </remarks>
        /// <param name="eventId">      Identifier for the event. </param>
        /// <param name="claims">   The user claims. </param>
        /// <returns>   The event. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public EventResponse GetEvent(Guid eventId, IEnumerable<Claim> claims)
        {
            try
            {
                var response = new EventResponse();

                if (eventId == Guid.Empty)
                    response.Result.Status = RequestResultStatus.InvalidParameter;

                if (!claims.ToList().TryGetValue<Guid>(BierAlyzerClaim.UserId, out var userId))
                    response.Result.Status = RequestResultStatus.TokenError;

                if (!claims.TryGetValue<UserType>(BierAlyzerClaim.UserType, out var userType))
                    response.Result.Status = RequestResultStatus.TokenError;

                // Does anything went wrong?
                if (!response.Result.Success) return response;

                // Try get event from database
                Event contextEvent;
                if (userType == UserType.Admin)
                    contextEvent = Context.Event.FirstOrDefault(x => x.EventId == eventId);
                else
                    contextEvent = Context.Event.Where(e => e.OwnerId == userId || e.EventUsers.Any(eu => eu.UserId == userId))
                        .FirstOrDefault(x => x.EventId == eventId);

                if (contextEvent != null)
                    response.Events.Add(Mapper.Map<EventDto>(contextEvent));
                else
                    response.Result.Status = RequestResultStatus.NoContent;


                return response;
            }
            catch (Exception e)
            {
                return new EventResponse
                {
                    Result = new RequestResult(RequestResultStatus.ServerError, e.Message)
                };
            }
        }

        public EventResponse JoinEvent(JoinEventRequest request, IEnumerable<Claim> claims)
        {
            var response = new EventResponse();
            if (!claims.TryGetValue<Guid>(BierAlyzerClaim.UserId, out var userId))
            {
                response.Result.Status = RequestResultStatus.TokenError;
                return response;
            }

            var contextEvent = Context.Event.FirstOrDefault(e => e.Code.ToLower() == request.Code.ToLower() && e.Type != EventType.Hidden);
            if (contextEvent == null)
            {
                response.Result.Status = RequestResultStatus.NotFound;
                return response;
            }

            if (!Context.UserEvent.Any(ue => ue.UserId == userId && ue.EventId == contextEvent.EventId))
            {
                Context.UserEvent.Add(new UserEvent { UserId = userId, EventId = contextEvent.EventId });
                Context.SaveChanges();
            }

            response.Events.Add(Mapper.Map<EventDto>(contextEvent));
            return response;
        }

        public EventResponse JoinPublicEvent(Guid eventId, IEnumerable<Claim> claims)
        {
            var response = new EventResponse();
            if (!claims.TryGetValue<Guid>(BierAlyzerClaim.UserId, out var userId))
            {
                response.Result.Status = RequestResultStatus.TokenError;
                return response;
            }

            var contextEvent = Context.Event.FirstOrDefault(e => e.EventId == eventId && e.Type == EventType.Public);
            if (contextEvent == null)
            {
                response.Result.Status = RequestResultStatus.NotFound;
                return response;
            }

            if (!Context.UserEvent.Any(ue => ue.UserId == userId && ue.EventId == contextEvent.EventId))
            {
                Context.UserEvent.Add(new UserEvent { UserId = userId, EventId = contextEvent.EventId });
                Context.SaveChanges();
            }

            response.Events.Add(Mapper.Map<EventDto>(contextEvent));
            return response;
        }

        public EventResponse LeaveEvent(Guid eventId, IEnumerable<Claim> claims)
        {
            var response = new EventResponse();
            if (!claims.TryGetValue<Guid>(BierAlyzerClaim.UserId, out var userId))
            {
                response.Result.Status = RequestResultStatus.TokenError;
                return response;
            }

            var userEvent = Context.UserEvent.FirstOrDefault(ue => ue.UserId == userId && ue.EventId == eventId);
            if (userEvent != null)
            {
                Context.UserEvent.Remove(userEvent);
                Context.SaveChanges();
            }
            return response;
        }

        public EventResponse CreateEvent(CreateEventRequest request, IEnumerable<Claim> claims)
        {
            var response = new EventResponse();
            if (!claims.TryGetValue<Guid>(BierAlyzerClaim.UserId, out var userId))
            {
                response.Result.Status = RequestResultStatus.TokenError;
                return response;
            }

            var newEvent = new Event
            {
                Name = request.Name,
                Description = request.Description,
                Created = DateTime.Now,
                Modified = DateTime.Now,
                Code = EventHelper.GenerateCode(Context),
                Start = request.Start,
                End = request.End,
                Type = EventType.Private,
                OwnerId = userId,
                Status = EventStatus.Open
            };

            Context.Event.Add(newEvent);
            Context.SaveChanges();

            response.Events.Add(Mapper.Map<EventDto>(newEvent));
            return response;
        }

        public EventResponse UpdateEvent(UpdateEventRequest request, IEnumerable<Claim> claims)
        {
            var response = new EventResponse();
            if (!claims.TryGetValue<Guid>(BierAlyzerClaim.UserId, out var userId))
            {
                response.Result.Status = RequestResultStatus.TokenError;
                return response;
            }

            var contextEvent = Context.Event.FirstOrDefault(e => e.EventId == request.EventId && e.OwnerId == userId);
            if (contextEvent == null)
            {
                response.Result.Status = RequestResultStatus.NotFound;
                return response;
            }

            if (!string.IsNullOrWhiteSpace(request.Name)) contextEvent.Name = request.Name;
            if (request.Description != null) contextEvent.Description = request.Description;
            if (request.Start.HasValue) contextEvent.Start = request.Start.Value;
            if (request.End.HasValue) contextEvent.End = request.End.Value;
            if (request.Type.HasValue) contextEvent.Type = request.Type.Value;

            contextEvent.Modified = DateTime.Now;
            Context.SaveChanges();

            response.Events.Add(Mapper.Map<EventDto>(contextEvent));
            return response;
        }

        public EventResponse SetStatus(SetEventStatusRequest request, IEnumerable<Claim> claims)
        {
            var response = new EventResponse();
            if (!claims.TryGetValue<Guid>(BierAlyzerClaim.UserId, out var userId))
            {
                response.Result.Status = RequestResultStatus.TokenError;
                return response;
            }

            var contextEvent = Context.Event.FirstOrDefault(e => e.EventId == request.EventId && e.OwnerId == userId);
            if (contextEvent == null)
            {
                response.Result.Status = RequestResultStatus.NotFound;
                return response;
            }

            if (request.Status == EventStatus.Open)
            {
                if (contextEvent.Start > DateTime.Now) contextEvent.Start = DateTime.Now.AddMinutes(-1);
                contextEvent.End = DateTime.Now.AddDays(1);
            }
            else if (request.Status == EventStatus.Closed)
            {
                contextEvent.End = DateTime.Now.AddMinutes(-1);
                if (contextEvent.Start > contextEvent.End) contextEvent.Start = DateTime.Today;
            }

            contextEvent.Status = request.Status;
            Context.SaveChanges();

            response.Events.Add(Mapper.Map<EventDto>(contextEvent));
            return response;
        }

        public EventResponse BookDrink(BookDrinkRequest request, IEnumerable<Claim> claims)
        {
            var response = new EventResponse();
            if (!claims.TryGetValue<Guid>(BierAlyzerClaim.UserId, out var userId))
            {
                response.Result.Status = RequestResultStatus.TokenError;
                return response;
            }

            var contextEvent = Context.Event.FirstOrDefault(e => e.EventId == request.EventId);
            if (contextEvent == null || contextEvent.Status != EventStatus.Open)
            {
                response.Result.Status = RequestResultStatus.InvalidParameter;
                return response;
            }

            // Check if user is part of event
            if (!Context.UserEvent.Any(ue => ue.UserId == userId && ue.EventId == request.EventId))
            {
                response.Result.Status = RequestResultStatus.Forbidden;
                return response;
            }

            if (!Context.Drink.Any(d => d.DrinkId == request.DrinkId))
            {
                response.Result.Status = RequestResultStatus.InvalidParameter;
                return response;
            }

            var drinkEntry = new DrinkEntry
            {
                DrinkId = request.DrinkId,
                UserId = userId,
                EventId = request.EventId
            };

            Context.DrinkEntry.Add(drinkEntry);
            Context.SaveChanges();

            return response;
        }

        public EventResponse RemoveEvent(Guid eventId, IEnumerable<Claim> claims)
        {
            var response = new EventResponse();
            if (!claims.TryGetValue<Guid>(BierAlyzerClaim.UserId, out var userId))
            {
                response.Result.Status = RequestResultStatus.TokenError;
                return response;
            }

            var contextEvent = Context.Event.FirstOrDefault(e => e.EventId == eventId && e.OwnerId == userId);
            if (contextEvent != null)
            {
                Context.Event.Remove(contextEvent);
                Context.SaveChanges();
            }
            return response;
        }
    }
}
