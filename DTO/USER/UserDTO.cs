using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.USER
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public int MaxConversionsAllowed { get; set; }
        public int ConversionsUsed { get; set; } // Opcional: para seguimiento del límite
    }
}
