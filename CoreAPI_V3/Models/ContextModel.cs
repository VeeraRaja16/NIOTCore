namespace CoreAPI_V3.Models
{
    public class ContextModel
    {
    }
        public class User
        {
            public int Id { get; set; }
            public string Email { get; set; }
            public string? Name { get; set; }
            public string? Locale { get; set; }
            public string? EmailVerified { get; set; }
            public string? RefreshToken { get; set; }
            public DateTime? RefreshTokenExpiryTime { get; set; }
            public string Role { get; set; }
            public DateTime SessionStart { get; set; }
        }

        public class Admin
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public byte[] PasswordHash { get; set; }
            public byte[] PasswordSalt { get; set; }
            public DateTime CreatedDate { get; set; }
            public string? RefreshToken { get; set; }
            public DateTime? RefreshTokenExpiryTime { get; set; }
            public string Role { get; set; }
            public DateTime SessionStart { get; set; }

        }


}
