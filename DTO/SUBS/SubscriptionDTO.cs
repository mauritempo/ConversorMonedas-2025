using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.SUBS
{
    public class SubscriptionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }          // Nombre de la suscripción (Free, Trial, Pro)
        public int MaxConversions { get; set; }    // Límite de conversiones permitido para esta suscripción
    }
}
