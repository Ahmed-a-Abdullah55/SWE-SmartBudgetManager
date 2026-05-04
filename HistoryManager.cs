using System;
using System.Collections.Generic;
using System.Linq;
using Masroofy.Models;
using Masroofy.Data;

namespace Masroofy.Services
{
    
    public class HistoryManager
    {
        private readonly DatabaseManager _db;

       
        public HistoryManager(DatabaseManager db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

       
        public List<Transaction> GetHistory()
        {
            List<Transaction> all = _db.LoadTransactions();

            return all
                .OrderByDescending(t => t.Date)
                .ToList();
        }


        public List<Transaction> FilterByCategory(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                return GetHistory();   

            List<Transaction> all = _db.LoadTransactions();

            return all
                .Where(t => string.Equals(t.Category, categoryName, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(t => t.Date)
                .ToList();
        }

        
        public List<Transaction> FilterByDate(DateTime from, DateTime to)
        {
            if (from > to)
                throw new ArgumentException(
                    $"'from' ({from:d}) must not be later than 'to' ({to:d}).");

            List<Transaction> all = _db.LoadTransactions();

            return all
                .Where(t => t.Date.Date >= from.Date && t.Date.Date <= to.Date)
                .OrderByDescending(t => t.Date)
                .ToList();
        }

        
        public double GetTotalSpent(List<Transaction> transactions)
        {
            if (transactions == null || transactions.Count == 0)
                return 0.0;

            return transactions.Sum(t => t.Amount);
        }
    }
}
