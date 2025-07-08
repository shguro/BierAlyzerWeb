namespace BierAlyzer.Api.DTOs
{
    /// <summary>
    /// DTO for login requests.
    /// </summary>
    public class LoginRequestDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
