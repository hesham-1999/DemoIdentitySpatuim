using Microsoft.AspNetCore.Identity;

namespace DemoIdentity.Model
{
    public class AppRole :IdentityRole
    {
        public string Description { get; set; }
    }
}
