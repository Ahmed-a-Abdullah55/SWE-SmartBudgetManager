using System;

namespace Masroofy.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }

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
