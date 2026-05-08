using System;
using System.Collections.Generic;
using Masroofy.Models;

namespace Masroofy.Services
{

    /// <summary>
    /// Checks budget thresholds and emits alerts when usage reaches warning or critical levels.
    /// </summary>
    public class AlertManager
    {
        private const double WarningThresholdPercent  = 80.0;
        private const double ExhaustedThresholdPercent = 100.0;


        /// <summary>
        /// Checks the current spend against the total budget and triggers alerts if thresholds are crossed.
        /// </summary>
        /// <param name="spent">Amount spent so far.</param>
        /// <param name="total">Total budget.</param>
        public void CheckThreshold(double spent, double total)
        {
            if (total <= 0)
                return;

            double percentage = (spent / total) * 100.0;

            if (percentage >= ExhaustedThresholdPercent)
            {
                SendBudgetExhaustedAlert(spent, total);
            }
            else if (percentage >= WarningThresholdPercent)
            {
                Send80PercentAlert(percentage);
            }
        }

        /// <summary>
        /// Sends a warning when approximately 80% of the budget is used.
        /// </summary>
        /// <param name="percentage">Percentage of budget used.</param>
        public void Send80PercentAlert(double percentage)
        {
            string message = $"Budget Warning: You have used {percentage:F1}% of your budget. "
                           + "Consider reducing your spending for the remaining days.";

            TriggerAlert(AlertType.Warning, message);
        }


        /// <summary>
        /// Sends a critical alert when the budget is exhausted or exceeded.
        /// </summary>
        /// <param name="spent">Amount spent.</param>
        /// <param name="total">Total budget.</param>
        public void SendBudgetExhaustedAlert(double spent, double total)
        {
            double overspend = spent - total;
            string message = overspend > 0
                ? $"Budget Exhausted: You have exceeded your budget by {overspend:F2}. "
                + "No further spending is recommended."
                : "Budget Exhausted: You have used 100% of your budget.";

            TriggerAlert(AlertType.Critical, message);
        }


        /// <summary>
        /// Writes the alert to the console with color coding.
        /// </summary>
        /// <param name="type">Alert severity.</param>
        /// <param name="message">Alert message text.</param>
        private void TriggerAlert(AlertType type, string message)
        {
            ConsoleColor original = Console.ForegroundColor;
            Console.ForegroundColor = type == AlertType.Critical
                ? ConsoleColor.Red
                : ConsoleColor.Yellow;

            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{type.ToString().ToUpper()}] {message}");
            Console.ForegroundColor = original;
        }


        /// <summary>
        /// Local alert severity enumeration.
        /// </summary>
        private enum AlertType
        {
            Warning,
            Critical
        }
    }
}
