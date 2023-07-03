using CleanArchMvc.API.Models;
using CleanArchMvc.Domain.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CleanArchMvc.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IAuthenticate _authentication;
        private readonly IConfiguration _configuration;

        public TokenController(IAuthenticate authentication, IConfiguration configuration)
        {
            _authentication = authentication ?? throw new ArgumentNullException(nameof(authentication));
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserToken>> Login([FromBody] LoginModel userInfo)
        {
            var result = await _authentication.Authenticate(userInfo.Email, userInfo.Password);

            if (result)
            {
                return GenerateToken(userInfo);
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Invalid login attempt");
                return BadRequest(ModelState);
            }
        }

        [HttpPost("CreateUser")]
        [ApiExplorerSettings(IgnoreApi = true)] // Não mostrar no swagger
        //[Authorize] // Para assegurar segurançar
        public async Task<ActionResult> CreateUser([FromBody] LoginModel userInfo)
        {
            var result = await _authentication.RegisterUser(userInfo.Email, userInfo.Password);

            if (result)
            {
                return Ok($"User {userInfo.Email} was created successfully");
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Invalid create user attempt");
                return BadRequest(ModelState);
            }
        }


        private UserToken GenerateToken(LoginModel userInfo)
        {
            // declaracao usuario
            var claims = new[]
            {
                new Claim("email", userInfo.Email),
                new Claim("meuvalor", "o que vc quiser"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // gerar chave privada para assinar o token
            var privateKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

            // gerar assinatura digital do token
            var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.HmacSha256);

            // definir tempo de expiração do token
            var expiration = DateTime.UtcNow.AddMinutes(10);

            // gerar o token
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
