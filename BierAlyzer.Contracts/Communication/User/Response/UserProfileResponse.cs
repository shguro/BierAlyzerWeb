using BierAlyzer.Contracts.Dto;
using BierAlyzer.Contracts.Interface.Communication;
using BierAlyzer.Contracts.Model;

namespace BierAlyzer.Contracts.Communication.User.Response
{
    public class UserProfileResponse : IApiResponseParameter
    {
        public UserDto User { get; set; }

        public RequestResult Result { get; set; }

        public UserProfileResponse()
        {
            Result = new RequestResult();
        }
    }
}
