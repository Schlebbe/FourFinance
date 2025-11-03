using FourFinance.Helpers;
using FourFinance.Users;
using Spectre.Console;

namespace FourFinance.Accounts
{
    public class Account
    {
        public Guid Id { get; set; }
        public int AccountNumber { get; set; }
        private decimal _balance;
        private Currency _currency;
        //public List<Loan> Loans { get; set; }
        //public List<Log> Logs { get; set; }

        public Account(int accountNumber, Currency currency)
        {
            Id = Guid.NewGuid();
            AccountNumber = accountNumber;
            _currency = currency;
        }
        
        public bool Withdraw(decimal amount)
        {
            if (amount > _balance)
            {
                AnsiConsole.MarkupLine("[red]Insufficient funds.[/]");
                return false;
            }

            _balance -= amount;
            
            return true;
            //TODO: Log transaction
        }

        public bool Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                AnsiConsole.MarkupLine("[red]Deposit amount must be positive.[/]");
                return false;
            }

            _balance += amount;
            return true;
            //TODO: Log transaction
        }

        public bool Transfer(decimal amount, int accountNumber, Customer currentUser)
        {
            var targetAccount = currentUser.Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);

            if (targetAccount == null)
            {
                AnsiConsole.MarkupLine("[red]Could not find a account with the given account number.[/]");
                return false;
            }

            var result = Withdraw(amount);

            if (result == true)
            {
                targetAccount.Deposit(amount);
                AnsiConsole.MarkupLine($"[green]{amount} {GetCurrency()}[/] has been transferred to account:[blue] {accountNumber}[/]");
                return true;
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Transfer failed.[/]");
                return false;
            }

            //TODO: Log transaction?
        }

        public decimal GetBalance()
        {
            return _balance;
        }

        public Currency GetCurrency()
        {
            return _currency;
        }
    }
}
