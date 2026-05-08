using System;
using System.Collections.Generic;
using System.Linq;
using Masroofy.Models;
using Masroofy.Data;

namespace Masroofy.Services
{
    
    /// <summary>
    /// Provides history and filtering utilities over stored transactions.
    /// </summary>
    public class HistoryManager
    {
        private readonly DatabaseManager _db;

       
        /// <summary>
        /// Constructs a HistoryManager that uses the provided database manager.
        /// </summary>
        /// <param name="db">Database manager for loading transactions.</param>
        public HistoryManager(DatabaseManager db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

       
        /// <summary>
        /// Returns all transactions ordered by date descending.
        /// </summary>
        /// <returns>List of transactions.</returns>
        public List<Transaction> GetHistory()
        {
            List<Transaction> all = _db.LoadTransactions();

            return all
                .OrderByDescending(t => t.Date)
                .ToList();
        }


        /// <summary>
        /// Filters history by category name (case-insensitive).
        /// </summary>
        /// <param name="categoryName">Category to filter by; returns all if null/empty.</param>
        /// <returns>Filtered list of transactions.</returns>
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

        
        /// <summary>
        /// Filters history between the given dates (inclusive).
        /// </summary>
        /// <param name="from">Start date.</param>
        /// <param name="to">End date.</param>
        /// <returns>Filtered list of transactions.</returns>
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

        
        /// <summary>
        /// Calculates the total spent for a list of transactions.
        /// </summary>
        /// <param name="transactions">Transactions to sum.</param>
        /// <returns>Total spent as double.</returns>
        public double GetTotalSpent(List<Transaction> transactions)
        {
            if (transactions == null || transactions.Count == 0)
                return 0.0;

            return transactions.Sum(t => t.Amount);
        }
    }
}
