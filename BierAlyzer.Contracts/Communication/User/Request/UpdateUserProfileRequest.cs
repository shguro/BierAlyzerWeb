using System;

namespace BierAlyzer.Contracts.Communication.User.Request
{
    public class UpdateUserProfileRequest
    {
        public string Username { get; set; }
        public string Origin { get; set; }
        public string Password { get; set; }
    }
}
