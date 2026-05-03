using System;

namespace Masroofy.Models
{
    public class BudgetCycle
    {
        public double TotalBudget { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        public BudgetCycle(double totalBudget, DateTime startDate, DateTime endDate)
        {
            TotalBudget = totalBudget;
            StartDate = startDate;
            EndDate = endDate;
            IsActive = true;
        }

        public int RemainingDays()
        {
            int days = (EndDate - DateTime.Now).Days;
            if (days < 0) return 0;
            return days;
        }
    }
}
