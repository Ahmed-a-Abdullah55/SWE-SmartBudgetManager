using System;
using System.Collections.Generic;
using System.Linq;
using Masroofy.Models;

namespace Masroofy.Services
{
    public class DailyLimitCalculator
    {
        public double CalculateInitialDailyLimit(BudgetCycle cycle)
        {
            if (cycle == null) throw new ArgumentNullException(nameof(cycle));

            int totalDays = (cycle.EndDate.Date - cycle.StartDate.Date).Days + 1;
            if (totalDays <= 0 || cycle.TotalBudget <= 0)
                return 0.0;

            return cycle.TotalBudget / totalDays;
        }

       
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

        
        public double CalculateUpdatedDailyLimit(BudgetCycle cycle, IEnumerable<Transaction> transactions)
        {
            if (transactions == null) return CalculateUpdatedDailyLimit(cycle, 0.0);

            double spent = transactions.Sum(t => t.Amount);
            return CalculateUpdatedDailyLimit(cycle, spent);
        }
    }
}