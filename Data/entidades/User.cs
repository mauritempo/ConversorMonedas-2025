using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Data.Models;

namespace Data.entidades
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [EmailAddress]
        public bool canMakeConversions { get; set; }
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }  // Relación de usuario con suscripción
        public int ConversionsUsed { get; set; }  // Cantidad de conversiones usadas por el usuario

        public bool IsAdmin { get; set; }
        
    }


}



