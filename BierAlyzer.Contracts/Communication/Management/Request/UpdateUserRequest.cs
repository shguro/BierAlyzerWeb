using System;
using System.ComponentModel.DataAnnotations;
using BierAlyzer.Contracts.Model;

namespace BierAlyzer.Contracts.Communication.Management.Request
{
    public class UpdateUserRequest
    {
        [Required]
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Origin { get; set; }
        public UserType Type { get; set; }
        public bool Enabled { get; set; }
    }
}
