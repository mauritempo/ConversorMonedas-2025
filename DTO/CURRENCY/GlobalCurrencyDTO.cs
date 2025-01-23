using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.CURRENCY
{
    public class GlobalCurrencyDTO
    {
        public string Code { get; set; }
        public string Legend { get; set; }
        public string Symbol { get; set; }
        public decimal ConvertibilityIndex { get; set; }
    }
}
