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
        public bool IsActive { get; set; }
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }  // Relación de usuario con suscripción

        // Propiedad adicional para las conversiones realizadas
        public int ConversionsUsed { get; set; }  // Cantidad de conversiones usadas por el usuario

        // Propiedad calculada para determinar si puede convertir
        public bool CanConvert
        {
            get
            {
                if (Subscription != null && IsActive)
                {
                    if (ConversionsUsed >= Subscription.MaxConversions)
                    {
                        IsActive = false;  // Desactiva al usuario si alcanzó el límite
                        return false;
                    }
                    return true;
                }
                return false;
            }
        }
    }


}



