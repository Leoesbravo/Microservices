using MicroserviceAuthentication.Model;
using MicroserviceAuthentication.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MicroserviceAuthentication.Services
{
    public class JWTTokenGenerator : IJWTTokenGenerator
    {
        private readonly JwtOptions _jwtoptions;
        public JWTTokenGenerator(IOptions<JwtOptions> jwtoptions)
        {
            _jwtoptions = jwtoptions.Value;
        }
        public string GenerateToken(IdentityUser user)
        {

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtoptions.Secret);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                new Claim(JwtRegisteredClaimNames.Name,user.UserName)
            };
            var tokenDescripter = new SecurityTokenDescriptor()
            {
                Audience = _jwtoptions.Audience,
                Issuer = _jwtoptions.Issuer,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescripter);

            return tokenHandler.WriteToken(token);
        }
    }
}
