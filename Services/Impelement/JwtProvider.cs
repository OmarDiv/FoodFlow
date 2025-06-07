using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace FoodFlow.Services.Impelement
{
    public class JwtProvider : IJwtProvider
    {
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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("LlohKI6zKQojsQVBDUc6XVGPfiTga84R")); 

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expirationIn = 30; // Token expiration time
            var token = new JwtSecurityToken(
                issuer: "FoodFlowApp", // Issuer
                audience: "FoodFlow Users", // Audience
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationIn),
                signingCredentials: signingCredentials
            );
            return (token : new JwtSecurityTokenHandler().WriteToken(token),expirationIn: expirationIn * 60);
        }
    }
}
