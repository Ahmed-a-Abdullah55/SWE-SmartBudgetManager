using System;

namespace Masroofy.Models
{
    /// <summary>
    /// Represents a single expense transaction.
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Transaction identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Monetary amount of the transaction.
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Optional descriptive note.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Date and time when the transaction was created or recorded.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Category name of the transaction.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Constructs a transaction with the given values and sets the timestamp to now.
        /// </summary>
        /// <param name="id">Transaction id.</param>
        /// <param name="amount">Amount.</param>
        /// <param name="note">Note text.</param>
        /// <param name="category">Category name.</param>
        public Transaction(int id, double amount, string note, string category)
        {
            Id = id;
            Amount = amount;
            Note = note;
            Date = DateTime.Now;
            Category = category;
        }
    }
}
