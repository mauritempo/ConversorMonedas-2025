using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.SUBS
{
    public class UserSubStatusDTO
    {
        public int UserId { get; set; }     
        
        public bool IsActive { get; set; }
        public string SubscriptionName { get; set; }  
        public int MaxConversions { get; set; }       
        public int ConversionsUsed { get; set; }      
        public int ConversionsRemaining { get; set; } 
    }
}
