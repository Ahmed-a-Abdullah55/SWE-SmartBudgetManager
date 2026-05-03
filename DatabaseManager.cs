using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Masroofy.Models;

namespace Masroofy.Data
{
    public class DatabaseManager
    {
        private string cycleFile = "cycle.json";
        private string transactionsFile = "transactions.json";

        public void SaveCycle(BudgetCycle cycle)
        {
            string json = JsonSerializer.Serialize(cycle);
            File.WriteAllText(cycleFile, json);
        }

        public BudgetCycle LoadCycle()
        {
            if (!File.Exists(cycleFile))
                return null;

            string json = File.ReadAllText(cycleFile);
            return JsonSerializer.Deserialize<BudgetCycle>(json);
        }

        public void SaveTransactions(List<Transaction> transactions)
        {
            string json = JsonSerializer.Serialize(transactions);
            File.WriteAllText(transactionsFile, json);
        }

        public List<Transaction> LoadTransactions()
        {
            if (!File.Exists(transactionsFile))
                return new List<Transaction>();

            string json = File.ReadAllText(transactionsFile);
            return JsonSerializer.Deserialize<List<Transaction>>(json);
        }

        public void persistData(BudgetCycle currentCycle, List<Transaction> currentTransactions)
        {
            if (currentCycle != null)
                SaveCycle(currentCycle);

            if (currentTransactions != null)
                SaveTransactions(currentTransactions);
        }
    }
}