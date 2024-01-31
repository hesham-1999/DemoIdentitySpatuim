using Microsoft.AspNetCore.Identity;

namespace DemoIdentity.Model
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public string? OTP { get; set; }
        public DateTime? OTPGeneratedAt { get; set; }

    }
}
