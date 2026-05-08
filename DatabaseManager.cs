using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Masroofy.Models;

namespace Masroofy.Data
{
    /// <summary>
    /// Simple file-based persistence for budget cycle and transactions.
    /// </summary>
    public class DatabaseManager
    {
        private string cycleFile = "cycle.json";
        private string transactionsFile = "transactions.json";

        /// <summary>
        /// Saves the given budget cycle to disk as JSON.
        /// </summary>
        /// <param name="cycle">Cycle to save.</param>
        public void SaveCycle(BudgetCycle cycle)
        {
            string json = JsonSerializer.Serialize(cycle);
            File.WriteAllText(cycleFile, json);
        }

        /// <summary>
        /// Loads the budget cycle from disk if available.
        /// </summary>
        /// <returns>Deserialized <see cref="BudgetCycle"/> or null if not present.</returns>
        public BudgetCycle LoadCycle()
        {
            if (!File.Exists(cycleFile))
                return null;

            string json = File.ReadAllText(cycleFile);
            return JsonSerializer.Deserialize<BudgetCycle>(json);
        }

        /// <summary>
        /// Persists transactions to disk as JSON.
        /// </summary>
        /// <param name="transactions">List of transactions to save.</param>
        public void SaveTransactions(List<Transaction> transactions)
        {
            string json = JsonSerializer.Serialize(transactions);
            File.WriteAllText(transactionsFile, json);
        }

        /// <summary>
        /// Loads transactions from disk, returning an empty list if none exist.
        /// </summary>
        /// <returns>List of <see cref="Transaction"/>.</returns>
        public List<Transaction> LoadTransactions()
        {
            if (!File.Exists(transactionsFile))
                return new List<Transaction>();

            string json = File.ReadAllText(transactionsFile);
            return JsonSerializer.Deserialize<List<Transaction>>(json);
        }

        /// <summary>
        /// Persists cycle and transactions together, when available.
        /// </summary>
        /// <param name="currentCycle">Current cycle to save (may be null).</param>
        /// <param name="currentTransactions">Transactions to save (may be null).</param>
        public void persistData(BudgetCycle currentCycle, List<Transaction> currentTransactions)
        {
            if (currentCycle != null)
                SaveCycle(currentCycle);

            if (currentTransactions != null)
                SaveTransactions(currentTransactions);
        }
    }
}