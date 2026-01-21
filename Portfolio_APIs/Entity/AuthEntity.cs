namespace ProjectAPI.Entity
{
    public class AuthEntity
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // Store hashed password 
 
    }
}
