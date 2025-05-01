using Microsoft.AspNetCore.Identity;

namespace ElKood.Core.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public List<RefreshToken> RefreshTokens { get; set; }
    }
}
