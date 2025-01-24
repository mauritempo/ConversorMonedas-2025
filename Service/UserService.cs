using Data.entidades;
using Data.repository;
using DTO.USER;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
        public User CreateUser(UserRegistrationDTO registrationDto)
        {
            // Crear un nuevo usuario
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
            // Buscar el usuario por nombre de usuario
            var user = _userRepository.GetUserByUsername(username);

            // Verificar que el usuario existe y la contraseña es correcta
            if (user != null && user.Password == password)
            {
                return user; // Devolver el usuario si las credenciales son válidas
            }

            return null; // Devolver null si las credenciales no son válidas
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

    }
}
