using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public enum SubscriptionType
    {
        Free = 5,      // Límite de 5 conversiones
        Trial = 100,   // Límite de 100 conversiones
        Pro = int.MaxValue // Sin límite

    }
}
