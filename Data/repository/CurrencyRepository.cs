﻿using Data.entidades;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.repository
{
    public class CurrencyRepository
    {
        private readonly MonedasContext _context;
        public CurrencyRepository(MonedasContext context)
        {
            _context = context;
        }

        public IEnumerable<CUrrency> GetGlobalCurrencies()
        {
            return _context.currencyConversions
            .Where(c => c.Status != CurrencyStatus.NonFunctional)
            .ToList();
        }
        
        public CUrrency GetCurrencyById(int id)
        {
            var currency = _context.currencyConversions.Find(id);
            if (currency != null && currency.Status == CurrencyStatus.NonFunctional)
            {
                return null; // Retorna null si está en estado NonFunctional
            }

            return (currency);
        }
        public CUrrency GetCurrencyByIdDeleted(int id)
        {
            var currency = _context.currencyConversions.Find(id);
            if (currency != null && currency.Status == CurrencyStatus.Modification)
            {
                return null; // Retorna null si está en estado NonFunctional
            }

            return (currency);
        }

        public int AddCurrency(CUrrency currency)
        {
            _context.currencyConversions.Add(currency);
            _context.SaveChanges();
            return currency.Id;
        }
        public CUrrency UpdateCurrency(CUrrency currency)
        {
            var existingCurrency = _context.currencyConversions.Find(currency.Id);
            _context.currencyConversions.Update(currency);         
            _context.SaveChanges();
            return currency;
            
        }
       
        public CUrrency? DeleteCurrency(int id)
        {
            CUrrency? currency = _context.currencyConversions.Find(id);
            if (currency != null)
            {
                currency.Status = CurrencyStatus.NonFunctional;
                _context.SaveChanges();
            }
            return currency;

        }
        public CUrrency GetCurrencyByCode(string code)
        {
            
            return _context.currencyConversions
                    .AsEnumerable()
                    .FirstOrDefault(c => c.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

        }
    }
}
