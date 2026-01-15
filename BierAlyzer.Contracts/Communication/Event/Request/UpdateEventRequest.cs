using System;
using System.ComponentModel.DataAnnotations;
using BierAlyzer.Contracts.Model;

namespace BierAlyzer.Contracts.Communication.Event.Request
{
    public class UpdateEventRequest
    {
        [Required]
        public Guid EventId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public EventType? Type { get; set; }
    }
}
