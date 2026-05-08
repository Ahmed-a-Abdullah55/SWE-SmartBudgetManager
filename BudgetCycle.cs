using System;

namespace Masroofy.Models
{
    /// <summary>
    /// Represents a budget cycle with a total budget and date range.
    /// </summary>
    public class BudgetCycle
    {
        /// <summary>
        /// Total budget allocated for the cycle.
        /// </summary>
        public double TotalBudget { get; set; }

        /// <summary>
        /// Cycle start date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Cycle end date.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Indicates whether the cycle is currently active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Initializes a new budget cycle and marks it active.
        /// </summary>
        /// <param name="totalBudget">Total budget amount.</param>
        /// <param name="startDate">Cycle start date.</param>
        /// <param name="endDate">Cycle end date.</param>
        public BudgetCycle(double totalBudget, DateTime startDate, DateTime endDate)
        {
            TotalBudget = totalBudget;
            StartDate = startDate;
            EndDate = endDate;
            IsActive = true;
        }

        /// <summary>
        /// Returns the number of remaining days in the cycle (0 if passed).
        /// </summary>
        /// <returns>Remaining days as integer.</returns>
        public int RemainingDays()
        {
            int days = (EndDate - DateTime.Now).Days;
            if (days < 0) return 0;
            return days;
        }
    }
}
