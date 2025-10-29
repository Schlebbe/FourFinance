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

        public void Withdraw(decimal amount)
        {
            if (amount > _balance)
            {
                AnsiConsole.MarkupLine("Insufficient funds.");
                return;
            }

            _balance -= amount;
            AnsiConsole.MarkupLine($"{amount} withdrawn. Current balance: {_balance}");
            //TODO: Log transaction
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                AnsiConsole.MarkupLine("Deposit amount must be positive.");
                return;
            }

            _balance += amount;
            AnsiConsole.MarkupLine($"{amount} deposited. Current balance: {_balance}");
            //TODO: Log transaction
        }

        public void Transfer(decimal amount, int accountNumber)
        {
            var targetAccount = BankHelper.GetAccountByNumber(AccountNumber);

            if (targetAccount == null)
            {
                AnsiConsole.MarkupLine("Could not find the account number.");
                return;
            }
           
            Withdraw(amount);
            targetAccount.Deposit(amount);

            AnsiConsole.MarkupLine($"{amount} transferred to account: {AccountNumber}. Current balance: {_balance}");
            //TODO: Log transaction
        }

        public void Transfer(decimal amount, int accountNumber, Customer currentUser)
        {
            var targetAccount = currentUser.Accounts.FirstOrDefault(a => a.AccountNumber == AccountNumber);
           // ska man kanske hitta sina egna konton via namn istället?

            if (targetAccount == null)
            {

                AnsiConsole.MarkupLine("Could not find the account number among your own accounts.");
                return;

            }

            Withdraw(amount);
            targetAccount.Deposit(amount);
            AnsiConsole.MarkupLine($"{amount} has been transferred to your own account: {AccountNumber}");
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
