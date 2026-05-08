using System;
using System.Collections.Generic;
using System.Linq;
using Masroofy.Models;

namespace Masroofy.Services
{
    /// <summary>
    /// Calculates initial and updated daily spending limits based on the budget cycle and transactions.
    /// </summary>
    public class DailyLimitCalculator
    {
        /// <summary>
        /// Calculates the initial daily limit by dividing total budget by total days in the cycle.
        /// </summary>
        /// <param name="cycle">Budget cycle to calculate for.</param>
        /// <returns>Initial daily limit (0.0 if invalid cycle).</returns>
        public double CalculateInitialDailyLimit(BudgetCycle cycle)
        {
            if (cycle == null) throw new ArgumentNullException(nameof(cycle));

            int totalDays = (cycle.EndDate.Date - cycle.StartDate.Date).Days + 1;
            if (totalDays <= 0 || cycle.TotalBudget <= 0)
                return 0.0;

            return cycle.TotalBudget / totalDays;
        }

       
        /// <summary>
        /// Calculates an updated daily limit based on remaining budget after spent amount.
        /// </summary>
        /// <param name="cycle">Budget cycle.</param>
        /// <param name="spent">Amount already spent.</param>
        /// <returns>Updated daily limit (non-negative).</returns>
        public double CalculateUpdatedDailyLimit(BudgetCycle cycle, double spent)
        {
            if (cycle == null) throw new ArgumentNullException(nameof(cycle));

            double remainingBudget = cycle.TotalBudget - spent;
            int remainingDays = cycle.RemainingDays();

            if (remainingBudget <= 0)
                return 0.0;

            if (remainingDays <= 0)
                return remainingBudget; // single final allowance

            return remainingBudget / remainingDays;
        }

        
        /// <summary>
        /// Calculates updated daily limit using transaction list to compute spent amount.
        /// </summary>
        /// <param name="cycle">Budget cycle.</param>
        /// <param name="transactions">Transactions to sum.</param>
        /// <returns>Updated daily limit.</returns>
        public double CalculateUpdatedDailyLimit(BudgetCycle cycle, IEnumerable<Transaction> transactions)
        {
            if (transactions == null) return CalculateUpdatedDailyLimit(cycle, 0.0);

            double spent = transactions.Sum(t => t.Amount);
            return CalculateUpdatedDailyLimit(cycle, spent);
        }
    }
}