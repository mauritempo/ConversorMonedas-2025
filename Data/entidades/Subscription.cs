using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;

namespace Data.entidades
{
    public class Subscription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public string Name { get; set; }  // Nombre de la suscripción (Free, Trial, Pro)
        public int MaxConversions { get; set; }  // Límite de conversiones para esta suscripción

        public SubscriptionType SubscriptionType { get; set; }
    }
}
