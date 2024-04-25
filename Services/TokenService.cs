using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using brokenaccesscontrol.Models;
using DotNetVulnApp.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace brokenaccesscontrol.Services
{
     public static class TokenService
     {
        public static string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("fedaf7d8863b48e197b9287d492b708e");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role,  user.IsAdmin ? "admin":"user"),
                    new Claim("Password", user.Password),
                    new Claim("Active", user.Inativo.ToString())
                    
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
               
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public static string EncryptJWE(Message plaintext)
        {
            var encryptionKey = RSA.Create(3072); 
            var signingKey = ECDsa.Create(ECCurve.NamedCurves.nistP256); 

            var encryptionKid = "8524e3e6674e494f85c5c775dcd602c5";
            var signingKid = "29b4adf8bcc941dc8ce40a6d0227b6d3";

            var privateEncryptionKey = new RsaSecurityKey(encryptionKey) {KeyId = encryptionKid};
            var publicEncryptionKey = new RsaSecurityKey(encryptionKey.ExportParameters(false)) {KeyId = encryptionKid};
            var privateSigningKey = new ECDsaSecurityKey(signingKey) {KeyId = signingKid};
            var publicSigningKey = new ECDsaSecurityKey(ECDsa.Create(signingKey.ExportParameters(false))) {KeyId = signingKid};

            var handler = new JsonWebTokenHandler();

            string token = handler.CreateToken(new SecurityTokenDescriptor
            {
                Audience = "teste",
                Issuer = "https://example.com",
                Claims = new Dictionary<string, object> { 
                    { "text", plaintext } },

                SigningCredentials = new SigningCredentials(
                    privateSigningKey, SecurityAlgorithms.EcdsaSha256),

    
                EncryptingCredentials = new EncryptingCredentials(
                    publicEncryptionKey, SecurityAlgorithms.RsaOAEP, SecurityAlgorithms.Aes256CbcHmacSha512)
            });

            return token;

        }
     }


     
}