using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Bookanizer.REST.Configuration;
using Bookanizer.REST.DAL.Models;
using Bookanizer.REST.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Bookanizer.REST.Services
{
    public class TokenService : ITokenService
    {
        #region Repositories, Services & Helpers
        private readonly JwtSettings _jwtSettings;
        #endregion

        #region Constructors
        public TokenService(IOptions<JwtSettings> jwtSettings) 
        {
            _jwtSettings = jwtSettings.Value ?? throw new ArgumentNullException(nameof(jwtSettings));
        }
        #endregion

        #region Methods
        public string GenerateJwt(UserModel user)
        {
            Claim[] claims =
            [
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            ];

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
    }
}
