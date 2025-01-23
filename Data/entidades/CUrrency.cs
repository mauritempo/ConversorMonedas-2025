using Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.entidades
{
    public class CUrrency
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Code { get; set; }          // Código de la moneda
        public string Legend { get; set; }        // Leyenda o nombre de la moneda
        public string Symbol { get; set; }        // Símbolo de la moneda
        public decimal ConvertibilityIndex { get; set; } // Índice de convertibilidad respecto al USD
        public CurrencyStatus Status { get; set; } 

    }

}
