using System;
using System.ComponentModel.DataAnnotations;
using BierAlyzer.Contracts.Model;

namespace BierAlyzer.Contracts.Communication.Event.Request
{
    public class SetEventStatusRequest
    {
        [Required]
        public Guid EventId { get; set; }
        [Required]
        public EventStatus Status { get; set; }
    }
}
