using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
namespace WebTest.Abstractions
{
    public interface IExtensionHelper
    {
        public ExtensionInfo GetExtension(byte[] data);
    }
    public class ExtensionInfo
    {
        public string Extension { get; set; }
        public string ContentType { get; set; }
    }


    public class TokenService
    {
        private readonly IHttpContextAccessor _context;

        public TokenService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public string CreateToken(IDictionary<string, object> claims)
        {
            var token = new SecurityTokenDescriptor
            {
                Issuer = "localhost",
                Audience = "reportsAudience",
                Claims = claims,
                Subject = new ClaimsIdentity(claims.Select(c => new Claim(c.Key, c.Value.ToString()))),
                Expires = DateTime.Now.AddMinutes(5)
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            jwtSecurityTokenHandler.InboundClaimTypeMap.Clear();
            jwtSecurityTokenHandler.MapInboundClaims = false;

            var jwtToken = jwtSecurityTokenHandler.CreateEncodedJwt(token);

            return jwtToken;
        }

        public JwtSecurityToken ValidateToken(string encryptedToken)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            jwtSecurityTokenHandler.InboundClaimTypeMap.Clear();
            jwtSecurityTokenHandler.MapInboundClaims = false;
            jwtSecurityTokenHandler.ValidateToken(
                encryptedToken,
                new TokenValidationParameters
                {
                    RequireSignedTokens = false,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidAudience = _context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value,
                    ValidIssuer = "reportsAudience"
                }, out SecurityToken validatedToken);

            return validatedToken as JwtSecurityToken;
        }
    }

    public interface IKeyMaterialService
    {
        EncryptingCredentials GetSigningCredentials();
        SecurityKey GetValidationKey();
    }
}
