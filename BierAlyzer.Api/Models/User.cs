namespace BierAlyzer.Api.Models
{
    /// <summary>
    /// Represents a user in the BierAlyzer system.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The unique identifier for the user.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The username for login.
        /// </summary>
        public string Username { get; set; } = string.Empty;
        /// <summary>
        /// The password hash (for demo, plain text; use hashing in production).
        /// </summary>
        public string Password { get; set; } = string.Empty;
        /// <summary>
        /// The display name of the user.
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;
    }
}
