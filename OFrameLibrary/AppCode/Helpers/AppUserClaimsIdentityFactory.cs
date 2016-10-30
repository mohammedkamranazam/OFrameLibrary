using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OFrameLibrary.Helpers
{
    public class AppUserClaimsIdentityFactory : ClaimsIdentityFactory<ApplicationUser, string>
    {
        public async override Task<ClaimsIdentity> CreateAsync(UserManager<ApplicationUser, string> manager, ApplicationUser user, string authenticationType)
        {
            var identity = await base.CreateAsync(manager, user, authenticationType);

            identity.AddClaim(new Claim(ClaimTypes.UserData, user.UserType));
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            identity.AddClaim(new Claim(ClaimTypes.MobilePhone, (string.IsNullOrWhiteSpace(user.PhoneNumber)) ? "" : user.PhoneNumber));

            return identity;
        }
    }
}