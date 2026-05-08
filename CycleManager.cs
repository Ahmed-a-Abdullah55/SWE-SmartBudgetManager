    using System;
using System.Collections.Generic;
using System.Linq;
using Masroofy.Data;
using Masroofy.Models;

namespace Masroofy.Services
{
    /// <summary>
    /// Manages budget cycles and transactions for the application.
    /// Provides operations to start, end, reset cycles and record transactions.
    /// </summary>
    public class CycleManager
    {
        private readonly DatabaseManager _db;
        private BudgetCycle _currentCycle;
        private readonly List<Transaction> _transactions;

        /// <summary>
        /// Initializes a new instance of <see cref="CycleManager"/>.
        /// Loads persisted cycle and transactions from the provided <paramref name="db"/>.
        /// </summary>
        /// <param name="db">Database manager used for persistence.</param>
        public CycleManager(DatabaseManager db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _currentCycle = _db.LoadCycle();
            _transactions = _db.LoadTransactions() ?? new List<Transaction>();
        }

        /// <summary>
        /// Returns the currently active or last known budget cycle.
        /// </summary>
        /// <returns>The current <see cref="BudgetCycle"/> or null if none exists.</returns>
        public BudgetCycle GetCurrentCycle()
        {
            return _currentCycle;
        }

        /// <summary>
        /// Returns the recorded transactions for the current cycle.
        /// </summary>
        /// <returns>Read-only list of <see cref="Transaction"/>.</returns>
        public IReadOnlyList<Transaction> GetTransactions()
        {
            return _transactions.AsReadOnly();
        }

        /// <summary>
        /// Indicates whether there is an active budget cycle that has not yet ended.
        /// </summary>
        /// <returns>True when a cycle is active and its end date has not passed; otherwise false.</returns>
        public bool IsCycleActive()
        {
            return _currentCycle != null && _currentCycle.IsActive && _currentCycle.EndDate >= DateTime.Now;
        }

        /// <summary>
        /// Starts a new budget cycle with the specified budget and date range.
        /// Clears existing transactions when a new cycle is started.
        /// </summary>
        /// <param name="totalBudget">Total budget for the new cycle (must be &gt; 0).</param>
        /// <param name="startDate">Start date of the new cycle.</param>
        /// <param name="endDate">End date of the new cycle (must be on or after <paramref name="startDate"/>).</param>
        /// <returns>The newly created <see cref="BudgetCycle"/>.</returns>
        /// <exception cref="ArgumentException">If budget &lt;= 0 or startDate &gt; endDate.</exception>
        /// <exception cref="InvalidOperationException">If there is already an active cycle.</exception>
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

        /// <summary>
        /// Ends the currently active cycle.
        /// </summary>
        /// <exception cref="InvalidOperationException">If there is no active cycle to end.</exception>
        public void EndCycle()
        {
            if (_currentCycle == null || !_currentCycle.IsActive)
                throw new InvalidOperationException("No active cycle to end.");

            _currentCycle.IsActive = false;
            PersistState();
        }

        /// <summary>
        /// Resets the cycle by creating a fresh active cycle and clearing transactions.
        /// </summary>
        /// <param name="totalBudget">Total budget for the reset cycle (must be &gt; 0).</param>
        /// <param name="startDate">Start date of the reset cycle.</param>
        /// <param name="endDate">End date of the reset cycle (must be on or after <paramref name="startDate"/>).</param>
        /// <returns>The newly created <see cref="BudgetCycle"/>.</returns>
        /// <exception cref="ArgumentException">If budget &lt;= 0 or startDate &gt; endDate.</exception>
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

        /// <summary>
        /// Adds a transaction to the current cycle.
        /// </summary>
        /// <param name="transaction">Transaction to add (must not be null).</param>
        /// <exception cref="ArgumentNullException">If <paramref name="transaction"/> is null.</exception>
        /// <exception cref="InvalidOperationException">If there is no active cycle.</exception>
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

        /// <summary>
        /// Computes the total amount spent in the current cycle based on recorded transactions.
        /// </summary>
        /// <returns>The sum of transaction amounts.</returns>
        public double GetTotalSpent()
        {
            return _transactions.Sum(t => t.Amount);
        }

        /// <summary>
        /// Returns the remaining budget for the current cycle. If overdrawn, returns 0.0.
        /// </summary>
        /// <returns>Remaining budget as non-negative double.</returns>
        public double GetRemainingBudget()
        {
            if (_currentCycle == null)
                return 0.0;

            double remaining = _currentCycle.TotalBudget - GetTotalSpent();
            return remaining < 0 ? 0.0 : remaining;
        }

        /// <summary>
        /// Persists current cycle and transactions to the database.
        /// </summary>
        private void PersistState()
        {
            _db.persistData(_currentCycle, _transactions);
        }
    }
}