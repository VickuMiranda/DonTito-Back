//using Microsoft.IdentityModel.Tokens;
//using Models.Models;
//using System.IdentityModel.Tokens.Jwt;
//using System.Runtime.Intrinsics.Arm;
//using System.Security.Claims;
//using System.Security.Cryptography;
//using System.Text;

//namespace DonTito.Custom
//{
//    public class Utilidades
//    {
//        private readonly IConfiguration _configuration; 
//        public Utilidades(IConfiguration configuration)
//        {
//            _configuration = configuration; 
//        }

//        //public string encriptarSHA256(string texto)
//        //{
//        //    using (SHA256 sha256HASH = SHA256.Create())
//        //    {
//        //        byte[] bytes = sha256HASH.ComputeHash(Encoding.UTF8.GetBytes(texto));

//        //        StringBuilder builder = new StringBuilder();
//        //        for(int i = 0; i < bytes.Length; i++)
//        //        {
//        //            builder.Append(bytes[i].ToString("X2"));
//        //        }
//        //        return builder.ToString();

//        //    }
//        //}

//        public string encriptarSHA256(string texto)
//            {
//                using (SHA256 sha256 = SHA256.Create())
//                {
//                    byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(texto));
//                    Span<char> hex = new char[bytes.Length * 2];
//                    for (int i = 0; i < bytes.Length; i++)
//                    {
//                        byte b = bytes[i];
//                        hex[i * 2] = GetHexValue(b >> 4);
//                        hex[i * 2 + 1] = GetHexValue(b & 0x0F);
//                    }
//                    return new string(hex);
//                }
//            }

//        public char GetHexValue(int value)
//        {
//            return (char)(value < 10 ? value + '0' : value - 10 + 'A');
//        }

//        public string generarJWT(Usuario usuario)
//            {
//                var userClaims = new[]
//                {
//                    new Claim("email", usuario.Email),
//                    new Claim("clave", usuario.Password!),

//                };

//                var securityKey = new SymmetricSecurityKey(
//                    Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:ClavePrivada").Get<string>() ?? string.Empty));

//                var credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

//                //crear detalle del token

//                var jwtConfig = new JwtSecurityToken(
//                    issuer: null,
//                audience: null,
//                userClaims,
//                expires: DateTime.Now.AddDays(1),
//                signingCredentials: credentials
//                );

//            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);

//            }



//        }
//}
using Microsoft.IdentityModel.Tokens;
using Models.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DonTito.Custom
{
    public class Utilidades
    {
        private readonly IConfiguration _configuration;

        public Utilidades(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Método para encriptar una cadena usando SHA256
        public string EncriptarSHA256(string texto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(texto));
                Span<char> hex = new char[bytes.Length * 2];
                for (int i = 0; i < bytes.Length; i++)
                {
                    byte b = bytes[i];
                    hex[i * 2] = GetHexValue(b >> 4);
                    hex[i * 2 + 1] = GetHexValue(b & 0x0F);
                }
                return new string(hex);
            }
        }

        // Método auxiliar para convertir un valor a su representación hexadecimal
        public char GetHexValue(int value)
        {
            return (char)(value < 10 ? value + '0' : value - 10 + 'A');
        }

        // Método para generar un JWT usando la información del usuario
        public string GenerarJWT(Usuario usuario)
        {
            var userClaims = new[]
            {
                new Claim("email", usuario.Email),
                new Claim("clave", usuario.Password!)
            };

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JWT:ClavePrivada"] ?? string.Empty));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Crear el token con los detalles y configuraciones especificadas
            var jwtConfig = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }
    }
}
