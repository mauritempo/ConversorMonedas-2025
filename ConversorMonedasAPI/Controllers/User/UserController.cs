using Data.entidades;
using DTO.SUBS;
using DTO.USER;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ConversorMonedasAPI.Controllers.User
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly IUserServices _userService;

        public UserController(IConfiguration config, IUserServices userService)
        {
            _config = config;
            _userService = userService;

        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Authenticate([FromBody] UserCredentials User)
        {
            var user = _userService.ValidateUser(User.Username, User.Password);

            if (user is null)
                return Unauthorized();

            //Paso 2: Crear el token
            var securityPassword = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Authentication:SecretForKey"]));

            var credentials = new SigningCredentials(securityPassword, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.Id.ToString()));
            claimsForToken.Add(new Claim("state", user.IsActive.ToString()));
            claimsForToken.Add(new Claim("Name", user.Username));

            var jwtSecurityToken = new JwtSecurityToken(
              _config["Authentication:Issuer"],
              _config["Authentication:Audience"],
              claimsForToken,
              DateTime.UtcNow,
              DateTime.UtcNow.AddHours(1),
              credentials);

            var tokenToReturn = new JwtSecurityTokenHandler()
                .WriteToken(jwtSecurityToken);

            // Devolver el token en formato JSON
            return Ok(new { token = tokenToReturn });

        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegistrationDTO registrationDto)
        {

            var user = _userService.CreateUser(registrationDto);

            if (user == null)
            {
                return BadRequest("User registration failed.");
            }

            return Ok("Hello: " + registrationDto.Username + ", welcome to currency APP");
        }


        [Authorize]
        [HttpGet("usuarios/detalles")]
        public IActionResult GetUserDetails()
        {
            // Extraer el token del encabezado
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            // Obtener el userIdSUB y username
            var userId = _userService.GetUserIdFromToken(token);

            var user = _userService.GetById(userId);

            if (userId == 0 & user == null)
            {
                return BadRequest("No se pudo extraer informacion del token");
            }
            // Retornar los detalles del usuario
            var userDetailsDto = new UserSubsDetails
            {
                Username = user.Username,
                SubscriptionId = user.SubscriptionId
            };

            return Ok(userDetailsDto);
        }
    }


    }



