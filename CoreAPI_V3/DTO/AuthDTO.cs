namespace CoreAPI_V3.DTO
{
    public class AuthDTO
    {
    }

    public class AdminLoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class GoogleAuthDto
    {
        public string IdToken { get; set; }
    }

}
