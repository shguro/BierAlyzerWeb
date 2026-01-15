using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using BierAlyzer.Api.Models;
using BierAlyzer.Contracts.Communication.Event;
using BierAlyzer.Contracts.Communication.Management.Request;
using BierAlyzer.Contracts.Communication.Management.Response;
using BierAlyzer.Contracts.Dto;
using BierAlyzer.Contracts.Model;
using BierAlyzer.EntityModel;
using Microsoft.EntityFrameworkCore;

namespace BierAlyzer.Api.Services
{
    public class ManagementService : BierAlyzerServiceBase
    {
        public ManagementService(BierAlyzerContext context, IMapper mapper) : base(context, mapper)
        {
        }

        private bool IsAdmin(IEnumerable<Claim> claims)
        {
            return claims.TryGetValue<UserType>(BierAlyzerClaim.UserType, out var type) && type == UserType.Admin;
        }

        public UsersResponse GetUsers(IEnumerable<Claim> claims)
        {
            var response = new UsersResponse();
            if (!IsAdmin(claims)) { response.Result.Status = RequestResultStatus.Forbidden; return response; }

            var users = Context.User.OrderByDescending(u => u.Created).ToList();
            response.Users = Mapper.Map<List<UserDto>>(users);
            return response;
        }

        public UsersResponse GetUser(Guid userId, IEnumerable<Claim> claims)
        {
            var response = new UsersResponse();
            if (!IsAdmin(claims)) { response.Result.Status = RequestResultStatus.Forbidden; return response; }

            var user = Context.User.FirstOrDefault(u => u.UserId == userId);
            if (user != null) response.Users.Add(Mapper.Map<UserDto>(user));
            else response.Result.Status = RequestResultStatus.NotFound;

            return response;
        }

        public UsersResponse UpdateUser(UpdateUserRequest request, IEnumerable<Claim> claims)
        {
            var response = new UsersResponse();
            if (!IsAdmin(claims)) { response.Result.Status = RequestResultStatus.Forbidden; return response; }

            var user = Context.User.FirstOrDefault(u => u.UserId == request.UserId);
            if (user == null) { response.Result.Status = RequestResultStatus.NotFound; return response; }

            user.Username = request.Username;
            user.Origin = request.Origin;
            user.Type = request.Type;
            user.Enabled = request.Enabled;
            user.Modified = DateTime.Now;

            Context.SaveChanges();
            response.Users.Add(Mapper.Map<UserDto>(user));
            return response;
        }

        public DrinkResponse GetDrinks(IEnumerable<Claim> claims)
        {
             var response = new DrinkResponse();
            if (!IsAdmin(claims)) { response.Result.Status = RequestResultStatus.Forbidden; return response; }

            var drinks = Context.Drink.OrderBy(d => d.Name).ToList();
            response.Drinks = Mapper.Map<List<DrinkDto>>(drinks);
            return response;
        }

        public DrinkResponse CreateDrink(CreateDrinkRequest request, IEnumerable<Claim> claims)
        {
             var response = new DrinkResponse();
            if (!IsAdmin(claims)) { response.Result.Status = RequestResultStatus.Forbidden; return response; }
            if (!claims.TryGetValue<Guid>(BierAlyzerClaim.UserId, out var userId)) { response.Result.Status = RequestResultStatus.TokenError; return response; }

            var drink = new Drink
            {
                Name = request.Name,
                Amount = request.Amount,
                Percentage = request.Percentage,
                Visible = request.Visible,
                Created = DateTime.Now,
                Modified = DateTime.Now,
                OwnerId = userId
            };

            Context.Drink.Add(drink);
            Context.SaveChanges();

            response.Drinks.Add(Mapper.Map<DrinkDto>(drink));
            return response;
        }

        public DrinkResponse UpdateDrink(UpdateDrinkRequest request, IEnumerable<Claim> claims)
        {
             var response = new DrinkResponse();
            if (!IsAdmin(claims)) { response.Result.Status = RequestResultStatus.Forbidden; return response; }

            var drink = Context.Drink.FirstOrDefault(d => d.DrinkId == request.DrinkId);
            if (drink == null) { response.Result.Status = RequestResultStatus.NotFound; return response; }

            drink.Name = request.Name;
            drink.Amount = request.Amount;
            drink.Percentage = request.Percentage;
            drink.Visible = request.Visible;
            drink.Modified = DateTime.Now;

            Context.SaveChanges();
            response.Drinks.Add(Mapper.Map<DrinkDto>(drink));
            return response;
        }

        public DrinkResponse DeleteDrink(Guid drinkId, IEnumerable<Claim> claims)
        {
             var response = new DrinkResponse();
            if (!IsAdmin(claims)) { response.Result.Status = RequestResultStatus.Forbidden; return response; }

            var drink = Context.Drink.Include(d => d.DrinkEntries).FirstOrDefault(d => d.DrinkId == drinkId);
            if (drink == null) { response.Result.Status = RequestResultStatus.NotFound; return response; }

            if (drink.DrinkEntries.Any())
            {
                 response.Result.Status = RequestResultStatus.Forbidden; // Cannot delete used drink
                 return response;
            }

            Context.Drink.Remove(drink);
            Context.SaveChanges();
            return response;
        }

        public EventResponse GetAllEvents(IEnumerable<Claim> claims)
        {
            var response = new EventResponse();
            if (!IsAdmin(claims)) { response.Result.Status = RequestResultStatus.Forbidden; return response; }

            var events = Context.Event.OrderByDescending(e => e.Created).ToList();
            response.Events.AddRange(Mapper.Map<List<EventDto>>(events));
            return response;
        }

        public EventResponse DeleteEvent(Guid eventId, IEnumerable<Claim> claims)
        {
            var response = new EventResponse();
            if (!IsAdmin(claims)) { response.Result.Status = RequestResultStatus.Forbidden; return response; }

            var contextEvent = Context.Event.FirstOrDefault(e => e.EventId == eventId);
            if (contextEvent == null) { response.Result.Status = RequestResultStatus.NotFound; return response; }

            Context.Event.Remove(contextEvent);
            Context.SaveChanges();

            return response;
        }
    }
}
