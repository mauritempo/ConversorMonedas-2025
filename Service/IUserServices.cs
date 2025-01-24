using Data.entidades;
using DTO.USER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IUserServices
    {
        User CreateUser(UserRegistrationDTO registrationDto);

        User GetById(int userId);
        User ValidateUser(string username, string password);

        User IncrementConversionsUsed(int userId);

        int GetUserIdFromToken(string token);
        User UpdateUser(User user);

        bool CanConvert(int userId);
    }
}