using DTO.CURRENCY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ICurrencyServices
    {
        List<CurrencyDTO> GetAllCurrencies();
        CurrencyDTO GetCurrencyById(int id);
        int AddCurrency(GlobalCurrencyDTO currencyDto);

        public bool UpdateCurrency(int id, GlobalCurrencyDTO currencyDto);
        bool UpdateCurrencyDeleted(int id, GlobalCurrencyDTO currencyDto);
        public bool DeleteCurrency(int id);
        ConversionResultDTO ConvertCurrency(ConversionRequestDTO request);
    }
}
