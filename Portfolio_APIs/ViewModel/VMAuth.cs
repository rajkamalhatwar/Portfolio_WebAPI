namespace ProjectAPI.ViewModel
{
    public class VMAuth
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    } 
    public class LoginResponseViewModel
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty; // JWT Token
        public int UserId { get; set; }
    }
}
