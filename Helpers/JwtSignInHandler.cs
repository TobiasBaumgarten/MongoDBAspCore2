using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Mongo.Helpers
{
    public class JwtSignInHandler
    {
        public const string TokenAudience = "Myself";
        public const string TokenIssuer = "MyProject";
        private readonly SymmetricSecurityKey key;

        public JwtSignInHandler(SymmetricSecurityKey symmetricKey)
        {
            this.key = symmetricKey;
        }

        public string BuildJwt(ClaimsPrincipal principal)
        {
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var token = new JwtSecurityToken(
                issuer: TokenIssuer,
                audience: TokenAudience,
                claims: principal.Claims,
                expires: DateTime.UtcNow.AddHours(20),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}