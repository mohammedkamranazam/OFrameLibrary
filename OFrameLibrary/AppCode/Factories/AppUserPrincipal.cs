using System.Security.Claims;

namespace OFrameLibrary.Factories
{
    public class AppUserPrincipal : ClaimsPrincipal
    {
        public AppUserPrincipal(ClaimsPrincipal principal)
            : base(principal)
        {
        }

        public string Email
        {
            get
            {
                return this.FindFirst(ClaimTypes.Email).Value;
            }
        }

        public string ID
        {
            get
            {
                return this.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
        }

        public string Name
        {
            get
            {
                return this.FindFirst(ClaimTypes.Name).Value;
            }
        }

        public string UserType
        {
            get
            {
                return this.FindFirst(ClaimTypes.UserData).Value;
            }
        }
    }
}
