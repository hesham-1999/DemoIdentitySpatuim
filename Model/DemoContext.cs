using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DemoIdentity.Model
{
    public class DemoContext : IdentityDbContext<AppUser,AppRole ,string>
    {

        public DemoContext(DbContextOptions<DemoContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            var roles = new AppRole[3];
            roles[0] = new AppRole() { Name = "SuperAdmin" ,Description ="test"};
            roles[1] = new AppRole() { Name = "Admin", Description = "test" };
            roles[2] = new AppRole() { Name = "owner", Description = "test" };

            builder.Entity<AppRole>().HasData(roles);
            base.OnModelCreating(builder);
        }
    }
}
