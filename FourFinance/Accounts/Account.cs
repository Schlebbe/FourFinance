using FourFinance.Helpers;
using Spectre.Console;

namespace FourFinance.Accounts
{
    public class Account
    {
        public Guid Id { get; set; }
        public int AccountNumber { get; set; }
        private decimal _balance;
        private string _currency;
        public List<Loan> Loans { get; set; } = new List<Loan>();
        public List<TransactionLog> Logs { get; set; } = new List<TransactionLog>();

        public Account(int accountNumber, string currency)
        {
            Id = Guid.NewGuid();
            AccountNumber = accountNumber;
            _currency = currency;
        }

        private void LogTransaction(string type, decimal amount, string message = "", int? targetAccountNumber = null)
        {
            Logs.Add(new TransactionLog
            {
                Timestamp = DateTime.Now,
                Type = type,
                Amount = amount,
                AccountNumber = AccountNumber,
                TargetAccountNumber = targetAccountNumber,
                Currency = _currency.ToString(),
                BalanceAfter = _balance,
                Message = message
            });
        }

        public bool Withdraw(decimal amount)
        {
            if (amount > _balance)
            {
                AnsiConsole.MarkupLine("[red]Insufficient funds.[/]");
                LogTransaction("Withdrawal Failed", amount, "Insufficient funds");
                return false;
            }

            _balance -= amount;
            LogTransaction("Withdrawal", amount, "Withdrawal successful");
            return true;
        }

        public virtual bool Deposit(decimal amount, bool shouldPrint, bool isInterest)
        {
            // Determine log type and message based on whether it's interest or a regular deposit using ternary operators
            var logType = isInterest ? "Interest" : "Deposit";
            var logMessage = isInterest ? "Interest applied" : "Deposit successful";

            if (amount <= 0)
            {
                if (shouldPrint)
                {
                    AnsiConsole.MarkupLine("[red]Deposit amount must be positive.[/]");
                }
                LogTransaction("Deposit Failed", amount, "Invalid deposit amount");
                return false;
            }

            _balance += amount;
            LogTransaction(logType, amount, logMessage);
            return true;
        }

        public bool Transfer(decimal amount, int accountNumber)
        {
            var targetAccount = BankHelper.GetAccountByNumber(accountNumber);

            if (targetAccount == null)
            {
                AnsiConsole.MarkupLine("[red]Could not find an account with the given account number.[/]");
                LogTransaction("Transfer Failed", amount, "Target account not found", accountNumber);
                return false;
            }

            if (targetAccount.AccountNumber == AccountNumber)
            {
                AnsiConsole.MarkupLine("[red]You cannot transfer to the same account.[/]");
                return false;
            }

            var result = Withdraw(amount);

            if (result == true)
            {
                // Check if currency conversion is needed
                if (targetAccount.GetCurrency() != GetCurrency())
                {
                    var convertedAmount = BankHelper.CalculateExchange(amount, GetCurrency(), targetAccount.GetCurrency());

                    // Deposit the converted amount into the target account without writing to the console or interest
                    targetAccount.Deposit(convertedAmount, false, false);
                    AnsiConsole.MarkupLine($"[green]{amount:F2} {GetCurrency()}[/] has been exchanged to [green]{convertedAmount:F2} {targetAccount.GetCurrency()}[/] and transferred to account:[blue] {accountNumber}[/]");
                    LogTransaction("Transfer", amount, "Transfer successful", accountNumber);
                    targetAccount.LogTransaction("Transfer In", convertedAmount, "Transfer received", accountNumber);
                    return true;
                }
                // No currency conversion needed

                // Deposit the amount into the target account without writing to the console or interest
                targetAccount.Deposit(amount, false, false);
                LogTransaction("Transfer", amount, "Transfer successful", accountNumber);
                AnsiConsole.MarkupLine($"[green]{amount:F2} {GetCurrency()}[/] has been transferred to account:[blue] {accountNumber}[/]");
                targetAccount.LogTransaction("Transfer In", amount, "Transfer received", accountNumber);
                return true;
            }
            else // Withdrawal failed
            {
                LogTransaction("Transfer Failed", amount, "Withdrawal failed", accountNumber);
                AnsiConsole.MarkupLine("[red]Transfer failed.[/]");
                return false;
            }
        }

        public decimal GetBalance()
        {
            return _balance;
        }

        public string GetCurrency()
        {
            return _currency;
        }

        public decimal GetActiveLoanAmount()
        {
            decimal totalLoanAmount = 0;
            foreach (var loan in Loans)
            {
                totalLoanAmount += loan.Principal;
            }
            return totalLoanAmount;
        }

        public void printLogs()
        {
            foreach (var log in Logs)
            {
                AnsiConsole.MarkupLine($"[grey]{log.Timestamp:G}[/] - [cyan]{log.Type}[/] - [green]{log.Amount:F2} {log.Currency}[/] - " +
                                       $"Balance: [blue]{log.BalanceAfter:F2}[/] - [white]{log.Message}[/]");
            }
            AnsiConsole.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            AnsiConsole.Clear();
        }
    }
}
