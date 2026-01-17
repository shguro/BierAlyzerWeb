using System.ComponentModel.DataAnnotations;

namespace BierAlyzer.Contracts.Communication.Management.Request
{
    public class CreateDrinkRequest
    {
        [Required]
        public string Name { get; set; }
        public double Amount { get; set; }
        public double Percentage { get; set; }
        public bool Visible { get; set; }
    }
}
