using System;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using BierAlyzer.Api.Helper;
using BierAlyzer.Api.Models;
using BierAlyzer.Contracts.Communication.User.Request;
using BierAlyzer.Contracts.Communication.User.Response;
using BierAlyzer.Contracts.Dto;
using BierAlyzer.Contracts.Model;
using BierAlyzer.EntityModel;

namespace BierAlyzer.Api.Services
{
    public class UserService : BierAlyzerServiceBase
    {
        public UserService(BierAlyzerContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public UserProfileResponse GetUserProfile(Claim[] claims)
        {
            var response = new UserProfileResponse();
            if (!claims.TryGetValue<Guid>(BierAlyzerClaim.UserId, out var userId))
            {
                response.Result.Status = RequestResultStatus.TokenError;
                return response;
            }

            var user = Context.User.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                response.Result.Status = RequestResultStatus.NotFound;
                return response;
            }

            response.User = Mapper.Map<UserDto>(user);
            return response;
        }

        public UserProfileResponse UpdateUserProfile(UpdateUserProfileRequest request, Claim[] claims)
        {
            var response = new UserProfileResponse();
            if (!claims.TryGetValue<Guid>(BierAlyzerClaim.UserId, out var userId))
            {
                response.Result.Status = RequestResultStatus.TokenError;
                return response;
            }

            var user = Context.User.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                response.Result.Status = RequestResultStatus.NotFound;
                return response;
            }

            // Update fields
            if (!string.IsNullOrWhiteSpace(request.Username))
                user.Username = request.Username;

            if (!string.IsNullOrWhiteSpace(request.Origin))
                user.Origin = request.Origin;

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                var salt = AuthenticationHelper.GenerateSalt();
                var hash = AuthenticationHelper.CalculatePasswordHash(salt, request.Password);
                user.Salt = salt;
                user.Hash = hash;
            }

            user.Modified = DateTime.Now;
            Context.SaveChanges();

            response.User = Mapper.Map<UserDto>(user);
            return response;
        }
    }
}
