using System.Collections.Generic;
using BierAlyzer.Contracts.Dto;
using BierAlyzer.Contracts.Interface.Communication;
using BierAlyzer.Contracts.Model;

namespace BierAlyzer.Contracts.Communication.Management.Response
{
    public class UsersResponse : IApiResponseParameter
    {
        public List<UserDto> Users { get; set; }
        public RequestResult Result { get; set; }

        public UsersResponse()
        {
            Users = new List<UserDto>();
            Result = new RequestResult();
        }
    }
}
