using Api.Models;
using System.Security.Claims;

namespace Api.Services.Authentication
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor context;

        public IdentityService(
            IHttpContextAccessor context)
        {
            this.context = context;
        }

        public IdentityModel GetIdentity()
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var identity = (ClaimsIdentity)context.HttpContext.User.Identity;

                return new IdentityModel
                {
                    UserName = identity.FindFirst(ClaimTypes.NameIdentifier).Value,
                };
            }

            return null;
        }
    }
}
