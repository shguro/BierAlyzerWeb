using System;
using System.ComponentModel.DataAnnotations;

namespace BierAlyzer.Contracts.Communication.Event.Request
{
    public class CreateEventRequest
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }
    }
}
