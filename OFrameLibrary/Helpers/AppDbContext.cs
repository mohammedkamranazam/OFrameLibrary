using Microsoft.AspNet.Identity.EntityFramework;

namespace OFrameLibrary.Helpers
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext()
            : base("DefaultConnection")
        {
        }

        public static AppDbContext Create()
        {
            return new AppDbContext();
        }
    }
}