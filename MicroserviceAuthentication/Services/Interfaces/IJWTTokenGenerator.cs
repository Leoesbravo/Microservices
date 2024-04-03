using Microsoft.AspNetCore.Identity;

namespace MicroserviceAuthentication.Services.Interfaces
{
    public interface IJWTTokenGenerator
    {
        string GenerateToken(IdentityUser user);
    }
}
