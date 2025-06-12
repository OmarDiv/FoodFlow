using FoodFlow.Contracts.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace FoodFlow.Services.Impelement
{
    public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
    {
        private readonly JwtOptions _options = options.Value;

        public (string token, int expirationIn) GenerateToken(ApplicationUser user)
        {
            Claim[] claims = new Claim[]
             {

                new Claim(JwtRegisteredClaimNames.Sub , user.Id),
                new Claim(JwtRegisteredClaimNames.Email , user.Email!),
                new Claim(JwtRegisteredClaimNames.GivenName , user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName , user.LastName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), 
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key)); 

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expirationIn =_options.ExpirationInMinutes; // Token expiration time
            var token = new JwtSecurityToken(
                issuer: _options.Issuer, // Issuer
                audience: _options.Audience, // Audience
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationIn),
                signingCredentials: signingCredentials
            );
            return (token : new JwtSecurityTokenHandler().WriteToken(token),expirationIn: expirationIn * 60);
        }
    }
}
