using System;
using System.Collections.Generic;
using System.Linq;
using Masroofy.Data;
using Masroofy.Models;

namespace Masroofy.Services
{
    public class ExpenseManager
    {
        private readonly DatabaseManager _db;
        private readonly List<Transaction> _transactions;

        public ExpenseManager(DatabaseManager db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _transactions = _db.LoadTransactions() ?? new List<Transaction>();
        }

        public IReadOnlyList<Transaction> GetAllTransactions()
        {
            return _transactions
                .OrderByDescending(t => t.Date)
                .ToList()
                .AsReadOnly();
        }

        public Transaction GetTransactionById(int id)
        {
            return _transactions.FirstOrDefault(t => t.Id == id);
        }

        public Transaction AddTransaction(double amount, string note, string category)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("Category must be provided.", nameof(category));

            int id = GetNextId();
            var transaction = new Transaction(id, amount, note ?? string.Empty, category);
            _transactions.Add(transaction);
            PersistTransactions();
            return transaction;
        }

        public void EditTransaction(int id, double amount, string note, string category, DateTime? date = null)
        {
            var tx = _transactions.FirstOrDefault(t => t.Id == id);
            if (tx == null)
                throw new InvalidOperationException($"Transaction with id {id} not found.");

            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("Category must be provided.", nameof(category));

            tx.Amount = amount;
            tx.Note = note ?? string.Empty;
            tx.Category = category;
            if (date.HasValue)
                tx.Date = date.Value;

            PersistTransactions();
        }

        public bool DeleteTransaction(int id)
        {
            var tx = _transactions.FirstOrDefault(t => t.Id == id);
            if (tx == null)
                return false;

            _transactions.Remove(tx);
            PersistTransactions();
            return true;
        }

        private int GetNextId()
        {
            return _transactions.Count == 0 ? 1 : _transactions.Max(t => t.Id) + 1;
        }

        private void PersistTransactions()
        {
            _db.SaveTransactions(_transactions);
        }
    }
}