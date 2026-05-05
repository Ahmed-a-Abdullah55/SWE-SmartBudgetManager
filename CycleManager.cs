using System;
using System.Collections.Generic;
using System.Linq;
using Masroofy.Data;
using Masroofy.Models;

namespace Masroofy.Services
{
    public class CycleManager
    {
        private readonly DatabaseManager _db;
        private BudgetCycle _currentCycle;
        private readonly List<Transaction> _transactions;

        public CycleManager(DatabaseManager db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _currentCycle = _db.LoadCycle();
            _transactions = _db.LoadTransactions() ?? new List<Transaction>();
        }

        public BudgetCycle GetCurrentCycle()
        {
            return _currentCycle;
        }

        public IReadOnlyList<Transaction> GetTransactions()
        {
            return _transactions.AsReadOnly();
        }

        public bool IsCycleActive()
        {
            return _currentCycle != null && _currentCycle.IsActive && _currentCycle.EndDate >= DateTime.Now;
        }

        public BudgetCycle StartCycle(double totalBudget, DateTime startDate, DateTime endDate)
        {
            if (totalBudget <= 0)
                throw new ArgumentException("Total budget must be greater than zero.", nameof(totalBudget));

            if (startDate > endDate)
                throw new ArgumentException("'startDate' must not be later than 'endDate'.");

            if (IsCycleActive())
                throw new InvalidOperationException("A budget cycle is already active. End or reset it before starting a new one.");

            _currentCycle = new BudgetCycle(totalBudget, startDate, endDate);
            // starting a new cycle clears existing transactions for the new cycle
            _transactions.Clear();

            PersistState();
            return _currentCycle;
        }

        public void EndCycle()
        {
            if (_currentCycle == null || !_currentCycle.IsActive)
                throw new InvalidOperationException("No active cycle to end.");

            _currentCycle.IsActive = false;
            PersistState();
        }

        public BudgetCycle ResetCycle(double totalBudget, DateTime startDate, DateTime endDate)
        {
            if (totalBudget <= 0)
                throw new ArgumentException("Total budget must be greater than zero.", nameof(totalBudget));

            if (startDate > endDate)
                throw new ArgumentException("'startDate' must not be later than 'endDate'.");

            // Resetting a cycle creates a fresh active cycle and clears transactions
            _currentCycle = new BudgetCycle(totalBudget, startDate, endDate);
            _transactions.Clear();

            PersistState();
            return _currentCycle;
        }

        public void AddTransaction(Transaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            // If there's no active cycle, still allow recording but warn by exception
            if (_currentCycle == null || !_currentCycle.IsActive)
                throw new InvalidOperationException("Cannot add transaction when there is no active cycle. Start a cycle first.");

            _transactions.Add(transaction);
            PersistState();
        }

        public double GetTotalSpent()
        {
            return _transactions.Sum(t => t.Amount);
        }

        public double GetRemainingBudget()
        {
            if (_currentCycle == null)
                return 0.0;

            double remaining = _currentCycle.TotalBudget - GetTotalSpent();
            return remaining < 0 ? 0.0 : remaining;
        }

        private void PersistState()
        {
            _db.persistData(_currentCycle, _transactions);
        }
    }
}