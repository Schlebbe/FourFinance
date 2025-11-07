using FourFinance.Accounts;
using FourFinance.Helpers;
using Spectre.Console;
namespace FourFinance.Users
{
    public class Customer : IUser
    {
        public Guid Id { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        
        public List<Account> Accounts { get; set; } = new List<Account>();

        public Customer(int age, string name, string email, string password, string userName)
        {
            Age = age;
            Name = name;
            Email = email;
            Password = password;
            UserName = userName;
            Id = Guid.NewGuid();
        }

        public void CreateAccount(string currency, bool isSavingsAccount)
        {
            var accountNumber = BankHelper.GenerateAccountNumber();

            if (isSavingsAccount) { 
                var newSavingsAccount = new SavingsAccount(accountNumber, currency);
                Accounts.Add(newSavingsAccount);
                AnsiConsole.MarkupLine($"Savings account created [green]successfully[/]. Account number: [blue]{accountNumber}[/]. Currency: [blue]{currency}[/].");
                return;
            }

            var newAccount = new Account(accountNumber, currency);
            Accounts.Add(newAccount);
            AnsiConsole.MarkupLine($"Checking account created [green]successfully[/]. Account number: [blue]{accountNumber}[/]. Currency: [blue]{currency}[/].");
        }

        public decimal CustomerAssets()
        {
            decimal assets = 0;
            foreach (var account in Accounts)
            {
                assets += account.GetBalance() - account.GetActiveLoanAmount();
            }
            return assets;
        }
    }
}
