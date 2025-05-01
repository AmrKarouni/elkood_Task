namespace ElKood.Application.Shared.Models.Dtos.Identity
{
    public class AuthenticationDto
    {
        public bool IsAuthenticated { get; set; }

        public string UserName { get; set; }

        public List<string> Roles { get; set; }

        public string Token { get; set; }

        public double TokenDurationM { get; set; }

        public DateTime? TokenExpiry { get; set; }

        public string RefreshToken { get; set; }

        public double RefreshTokenDurationM { get; set; }

        public DateTime RefreshTokenExpiry { get; set; }
    }
}
