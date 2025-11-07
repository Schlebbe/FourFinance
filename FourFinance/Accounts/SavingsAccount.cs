using FourFinance.Helpers;
using Spectre.Console;

namespace FourFinance.Accounts
{
    public class SavingsAccount : Account
    {
        public SavingsAccount(int accountNumber, string currency) : base(accountNumber, currency)
        { }

        public override bool Deposit(decimal amount, bool shouldLog, bool isInterest)
        {
            if (!base.Deposit(amount, shouldLog, isInterest))
            {
                return false;
            }

            if (!shouldLog)
            {
                return true;
            }

            var interest = amount * BankHelper.GetInterestRate();
            AnsiConsole.MarkupLine($"Your [green]interest[/] for this deposit is: [green]{interest:F2} {GetCurrency()}[/]");

            return true;
        }

        public void CalculateAndApplyInterest()
        {
            Deposit(GetBalance() * BankHelper.GetInterestRate(), false, true);
        }
    }
}
