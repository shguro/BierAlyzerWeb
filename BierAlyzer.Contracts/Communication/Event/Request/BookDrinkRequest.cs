using System;
using System.ComponentModel.DataAnnotations;

namespace BierAlyzer.Contracts.Communication.Event.Request
{
    public class BookDrinkRequest
    {
        [Required]
        public Guid EventId { get; set; }
        [Required]
        public Guid DrinkId { get; set; }
    }
}
