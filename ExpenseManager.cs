using System;
using System.Collections.Generic;
using System.Linq;
using Masroofy.Data;
using Masroofy.Models;

namespace Masroofy.Services
{
    /// <summary>
    /// Manages creation, editing and deletion of expense transactions.
    /// </summary>
    public class ExpenseManager
    {
        private readonly DatabaseManager _db;
        private readonly List<Transaction> _transactions;

        /// <summary>
        /// Constructs an ExpenseManager and loads persisted transactions.
        /// </summary>
        /// <param name="db">Database manager used for persistence.</param>
        public ExpenseManager(DatabaseManager db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _transactions = _db.LoadTransactions() ?? new List<Transaction>();
        }

        /// <summary>
        /// Returns all transactions ordered most-recent-first.
        /// </summary>
        /// <returns>Read-only list of transactions.</returns>
        public IReadOnlyList<Transaction> GetAllTransactions()
        {
            return _transactions
                .OrderByDescending(t => t.Date)
                .ToList()
                .AsReadOnly();
        }

        /// <summary>
        /// Finds a transaction by id.
        /// </summary>
        /// <param name="id">Transaction identifier.</param>
        /// <returns>Transaction or null if not found.</returns>
        public Transaction GetTransactionById(int id)
        {
            return _transactions.FirstOrDefault(t => t.Id == id);
        }

        /// <summary>
        /// Adds a new transaction to the store.
        /// </summary>
        /// <param name="amount">Amount of the transaction.</param>
        /// <param name="note">Optional note.</param>
        /// <param name="category">Category name.</param>
        /// <returns>Created transaction object.</returns>
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

        /// <summary>
        /// Edits an existing transaction.
        /// </summary>
        /// <param name="id">Transaction id.</param>
        /// <param name="amount">New amount.</param>
        /// <param name="note">New note.</param>
        /// <param name="category">New category.</param>
        /// <param name="date">Optional new date.</param>
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

        /// <summary>
        /// Deletes a transaction by id.
        /// </summary>
        /// <param name="id">Transaction id to delete.</param>
        /// <returns>True if deleted; false if not found.</returns>
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