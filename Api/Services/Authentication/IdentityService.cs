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

        public IdentityModel? GetIdentity()
        {
            if (context?.HttpContext?.User?.Identity?.IsAuthenticated == true)
            {
                var identity = (ClaimsIdentity)context.HttpContext.User.Identity;

                return new IdentityModel
                {
                    UserName = identity.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Id = Convert.ToInt32(identity.FindFirst("Id")?.Value)
                };
            }

            return null;
        }
    }
}
