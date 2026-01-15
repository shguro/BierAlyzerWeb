using System.ComponentModel.DataAnnotations;

namespace BierAlyzer.Contracts.Communication.Event.Request
{
    public class JoinEventRequest
    {
        [Required]
        public string Code { get; set; }
    }
}
