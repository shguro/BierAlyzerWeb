using System.ComponentModel.DataAnnotations;

namespace BierAlyzer.Contracts.Communication.Auth.Request
{
    public class RegisterRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Mail { get; set; }
        [Required]
        public string Origin { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
