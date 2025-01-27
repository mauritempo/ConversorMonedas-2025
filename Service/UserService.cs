using Data.entidades;
using Data.repository;
using DTO.USER;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;




namespace Service
{
    public class UserServices : IUserServices
    {
        private readonly UserRepository _userRepository;

        public UserServices(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public bool IsAdmin(int userId)
        {

            var user = _userRepository.GetById(userId);
            if (user == null) throw new ArgumentException("Usuario no encontrado.");
            return user.IsAdmin;
        }
        public User CreateAdminUser(AdminUserDTO adminUserDTO)
        {
            

            // Crear la entidad de usuario administrador
            var adminUser = new User
            {
                Username = adminUserDTO.Username,
                Password = adminUserDTO.Password, 
                Email = adminUserDTO.Email,
                canMakeConversions = true,
                IsAdmin = true,
                ConversionsUsed = 0,
                SubscriptionId = 3 
            };

            // Guardar en la base de datos
            _userRepository.AddUser(adminUser);

            return adminUser;
        }
        public User CreateUser(UserRegistrationDTO registrationDto)
        {
            
            var user = new User
            {
                Username = registrationDto.Username,
                Password = registrationDto.Password,
                Email = registrationDto.Email,
                SubscriptionId = registrationDto.SubscriptionId,
                canMakeConversions = true
            };

            _userRepository.AddUser(user);
            return user;
        }

        public User GetById(int userId)
        {
            return _userRepository.GetById(userId);
        }


        public User ValidateUser(string username, string password)
        {
            
            var user = _userRepository.GetUserByUsername(username);

            
            if (user != null && user.Password == password)
            {
                return user; 
            }

            return null; 
        }
        public User IncrementConversionsUsed(int userId)
        {
            var user = _userRepository.GetById(userId);

            if (user == null)
                throw new ArgumentException("Usuario no encontrado");

            // Incrementar conversiones usadas
            user.ConversionsUsed++;

            // Desactivar usuario si excede el límite
            if (user.ConversionsUsed >= user.Subscription.MaxConversions)
            {
                user.canMakeConversions = false;
            }

            // Actualizar en el repositorio
            _userRepository.Update(user);

            // Devolver el usuario actualizado
            return user;
        }

        public int GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            
            
            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);

                
                var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;

                if (int.TryParse(userIdClaim, out int userId))
                {
                    return userId;
                }
            }

            // Retorna null si no se pudo obtener el ID de usuario
            return 0;
        }

        public User UpdateUser(User user)
        {
            
            return _userRepository.Update(user);
        }
        public User UpdateUser(UpdateAdminDTO user, int adminUserId)
        {
            if (!IsAdmin(adminUserId))
            {
                throw new UnauthorizedAccessException("No tienes permisos para actualizar usuarios.");
            }
            var existingUser = _userRepository.GetById(user.Id);
            if (existingUser == null)
            {
                throw new ArgumentException("Usuario no encontrado.");
            }
            _userRepository.Update(existingUser);
            return existingUser;
        }

        public bool CanConvert(int userId)
        {
            var user = _userRepository.GetById(userId);
            if (user.Subscription != null && user.canMakeConversions)
            {
                if (user.ConversionsUsed >= user.Subscription.MaxConversions)
                {
                    user.canMakeConversions = false; // Desactivar excedió el límite
                    _userRepository.Update(user); // Actualizar el estado en la base de datos
                    return false;
                }
                return true; // Puede realizar conversiones
            }
            return false; // No puede realizar conversiones si no tiene una suscripción válida o está inactivo

        }
        public void DeactivateUser(int userId, int adminUserId)
        {
            if (!IsAdmin(adminUserId))
            {
                throw new UnauthorizedAccessException("No tienes permisos para desactivar usuarios.");
            }

            var user = _userRepository.GetById(userId);
            if (user == null) throw new ArgumentException("Usuario no encontrado.");

            user.canMakeConversions = false;
            _userRepository.Update(user);
        }

    }
}
