using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NutriPro.Application.Configurations.Filters
{
    public class JwtAuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly SymmetricSecurityKey _securityKey;

        public JwtAuthService(IConfiguration config)
        {
            _config = config;
            _tokenHandler = new JwtSecurityTokenHandler();
            _securityKey = GenerateSecurityKey();
        }

        private SymmetricSecurityKey GenerateSecurityKey()
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(_config["Authentication:JwtBearer:SecurityKey"]);
            return new SymmetricSecurityKey(keyBytes);
        }

        public string GenerateJwtToken(string email)
        {
            var credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
    new Claim(ClaimTypes.Name, email)
};

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _config["Authentication:JwtBearer:Issuer"],
                Audience = _config["Authentication:JwtBearer:Audience"],
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = credentials
            };

            var token = _tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var tokenString = _tokenHandler.WriteToken(token);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _securityKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            SecurityToken validatedToken;
            _tokenHandler.ValidateToken(tokenString, validationParameters, out validatedToken);

            return tokenString;

        }

        public bool ValidateToken(string token)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _securityKey,
                ValidateIssuer = true,
                ValidIssuer = _config["Authentication:JwtBearer:Issuer"],
                ValidateAudience = true,
                ValidAudience = _config["Authentication:JwtBearer:Audience"],
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                SecurityToken validatedToken;
                _tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                return validatedToken is JwtSecurityToken jwtToken;
            }
            catch (SecurityTokenException)
            {
                return false;
            }
        }
    }
}
