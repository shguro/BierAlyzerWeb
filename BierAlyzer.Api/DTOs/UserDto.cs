namespace BierAlyzer.Api.DTOs
{
    /// <summary>
    /// Data Transfer Object for User.
    /// </summary>
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
    }
}
