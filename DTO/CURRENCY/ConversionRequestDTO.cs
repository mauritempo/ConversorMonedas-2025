using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.CURRENCY
{
    public class ConversionRequestDTO
    {
        
        public string FromCurrencyCode { get; set; } 
        public string ToCurrencyCode { get; set; }   
        public decimal Amount { get; set; }         
    }
}
