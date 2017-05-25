using Microsoft.AspNet.Identity.EntityFramework;

namespace OFrameLibrary.Factories
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext()
            : base(AppConfig.AppDBContext)
        {
        }

        public static AppDbContext Create()
        {
            return new AppDbContext();
        }
    }
}
