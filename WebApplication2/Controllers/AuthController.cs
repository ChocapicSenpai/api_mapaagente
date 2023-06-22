using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication2.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Context;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AuthDbContext dbContext;


        public AuthController(IConfiguration configuration, AuthDbContext dbContext)
        {
            _configuration = configuration;
            this.dbContext = dbContext;

        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            // Aquí es donde validarás las credenciales y autenticarás al usuario

            // Ejemplo de validación básica (solo como referencia, debes implementar tu propia lógica)
            //if (loginModel.username == "admin" && loginModel.password == "password")
            if (IsValidAdmin(loginModel))
            {
                // Credenciales válidas, generar el token de autenticación
                var token = GenerateToken(loginModel.username);

                // Devolver el token como respuesta
                return Ok(new { token });
            }

            // Las credenciales son inválidas, devolver una respuesta de error
            return Unauthorized();
        }
        private bool IsValidAdmin(LoginModel loginModel)
        {
            // Aquí debes implementar la lógica para validar las credenciales del administrador
            // Puedes realizar una consulta a tu base de datos para verificar si existe un administrador con las credenciales proporcionadas
            // Por ejemplo, podrías utilizar Entity Framework para acceder a tu base de datos y buscar al administrador por nombre de usuario y contraseña
            // Si usas Entity Framework, asegúrate de tener el contexto de la base de datos configurado en tu proyecto
            var admin = dbContext.Set<LoginModel>().FirstOrDefault(a => a.username == loginModel.username && a.password == loginModel.password);

            // Devolver true si se encontró un administrador con las credenciales válidas, de lo contrario, devolver false
            return (admin != null);

            // Aquí hay un ejemplo básico de validación utilizando las credenciales fijas
            //return (loginModel.username == "admin" && loginModel.password == "password");
        }
        private string GenerateToken(string username)
        {
            // Aquí debes implementar la generación del token usando una biblioteca como JWT

            // Ejemplo básico usando la biblioteca System.IdentityModel.Tokens.Jwt
            var secretkey = _configuration["JwtSettings:SecretKey"];
            var key = Encoding.ASCII.GetBytes(secretkey);// Reemplaza con tu propia clave secreta

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }


    }
}


//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;
//using WebApplication2.Modelos;

//namespace WebApplication2.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AuthController : ControllerBase
//    {
//        private readonly IConfiguration _configuration;

//        public AuthController(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        [HttpPost("login")]
//        public async Task<IActionResult> Login(LoginModel loginModel)
//        {
//            if (ValidarCredenciales(loginModel))
//            {
//                var token = GenerarTokenJwt(loginModel.Username);
//                return Ok(new { token });
//            }

//            return Unauthorized(new { message = "Credenciales inválidas" });
//        }

//        private bool ValidarCredenciales(LoginModel loginModel)
//        {
//            // Aquí debes implementar la lógica para validar las credenciales del usuario
//            // Verificar el nombre de usuario y contraseña en tu sistema de autenticación
//            // Devolver true si son válidas, o false si no lo son

//            // Ejemplo de validación básica
//            return loginModel.Username == "admin" && loginModel.Password == "123456";
//        }

//        private string GenerarTokenJwt(string username)
//        {
//            var jwtSettings = _configuration.GetSection("JwtSettings");
//            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
//            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

//            var claims = new[]
//            {
//                new Claim(ClaimTypes.Name, username)
//            };

//            var token = new JwtSecurityToken(
//                issuer: jwtSettings["Issuer"],
//                audience: jwtSettings["Audience"],
//                claims: claims,
//                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpirationMinutes"])),
//                signingCredentials: signingCredentials
//            );

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }
//    }
//}
