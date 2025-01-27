using Data.entidades;
using DTO.USER;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace ConversorMonedasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserServices _userService;

        public AdminController(IConfiguration config, IUserServices userService)
        {
            _config = config;
            _userService = userService;

        }
        [HttpPost("create-admin")]
        public IActionResult CreateAdmin([FromBody] AdminUserDTO adminUserDTO)
        {
            // Extraer el token del encabezado
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            // Obtener el userIdSUB y username
            var admin = _userService.GetUserIdFromToken(token);

            // Validar si el usuario que realiza la solicitud es administrador
            if (!_userService.IsAdmin(admin))
            {
                return Unauthorized("No tienes permisos para crear un administrador.");
            }

            try
            {
                // Crear el usuario administrador
                var adminUser = _userService.CreateAdminUser(adminUserDTO);

                return CreatedAtAction(
                    nameof(Created), // Método que devuelve un administrador por su ID
                    new { id = adminUser.Id }, // Ruta del recurso recién creado
                    new
                    {
                        adminUser.Id,
                        adminUser.Username,
                        adminUser.Email
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("deactivate/{userId}")]
        public IActionResult DeactivateUser(int userId)
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                // Obtener el userIdSUB y username
                var admin = _userService.GetUserIdFromToken(token);

            if (!_userService.IsAdmin(admin))
            {
                return Unauthorized("No tienes permisos para crear un administrador.");
            }
            try
                {
                    _userService.DeactivateUser(userId, admin);
                    return Ok("Usuario desactivado correctamente.");
                }
                
                catch (ArgumentException )
                {
                    return BadRequest("faltan datos/no son correctos");
                }
            }

        [HttpPut("update")]
        public IActionResult UpdateUser([FromBody] UpdateAdminDTO updatedUser)
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            // Obtener el userIdSUB y username
            var admin = _userService.GetUserIdFromToken(token);

            if (!_userService.IsAdmin(admin))
            {
                return Unauthorized("No tienes permisos para crear un administrador.");
            }

            try
            {
                var user = _userService.UpdateUser(updatedUser,admin);
                return Ok(user);
            }

            catch (ArgumentException)
            {
                return BadRequest("faltan datos/no son correctos");
            }
        }

        [HttpPost("create")]
        public IActionResult CreateUser([FromBody] UserRegistrationDTO newUser)
        {
            
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            var admin = _userService.GetUserIdFromToken(token);

            if (!_userService.IsAdmin(admin))
            {
                return Unauthorized("No tienes permisos para crear un administrador.");
            }

            try
            {
                var createdUser = _userService.CreateUser(newUser);
                return Ok(createdUser);
            }
            catch (ArgumentException)
            {
                return BadRequest("faltan datos/no son correctos");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetAdminById(int id)
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                var admin = _userService.GetUserIdFromToken(token);
                if (!_userService.IsAdmin(admin))
                {
                        return NotFound("Administrador no encontrado.");
                }

                return Ok(admin);
            }


    }
}
