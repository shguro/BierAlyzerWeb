namespace BierAlyzer.Api.DTOs
{
    /// <summary>
    /// DTO for login responses.
    /// </summary>
    public class LoginResponseDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
