using System;
using System.Collections.Generic;
using Masroofy.Models;

namespace Masroofy.Services
{

    public class AlertManager
    {
        private const double WarningThresholdPercent  = 80.0;
        private const double ExhaustedThresholdPercent = 100.0;


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

        public void Send80PercentAlert(double percentage)
        {
            string message = $"Budget Warning: You have used {percentage:F1}% of your budget. "
                           + "Consider reducing your spending for the remaining days.";

            TriggerAlert(AlertType.Warning, message);
        }


        public void SendBudgetExhaustedAlert(double spent, double total)
        {
            double overspend = spent - total;
            string message = overspend > 0
                ? $"Budget Exhausted: You have exceeded your budget by {overspend:F2}. "
                + "No further spending is recommended."
                : "Budget Exhausted: You have used 100% of your budget.";

            TriggerAlert(AlertType.Critical, message);
        }


        private void TriggerAlert(AlertType type, string message)
        {
            ConsoleColor original = Console.ForegroundColor;
            Console.ForegroundColor = type == AlertType.Critical
                ? ConsoleColor.Red
                : ConsoleColor.Yellow;

            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{type.ToString().ToUpper()}] {message}");
            Console.ForegroundColor = original;
        }


        private enum AlertType
        {
            Warning,
            Critical
        }
    }
}
