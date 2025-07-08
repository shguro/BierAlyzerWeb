namespace BierAlyzer.Api.DTOs
{
    /// <summary>
    /// Data Transfer Object for Drink.
    /// </summary>
    public class DrinkDto
    {
        /// <summary>
        /// The unique identifier for the drink.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The name of the drink.
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// The alcohol by volume percentage.
        /// </summary>
        public double Abv { get; set; }
    }
}
