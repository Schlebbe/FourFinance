using FourFinance.Accounts;
using FourFinance.Users;
using Spectre.Console;

namespace FourFinance.Helpers
{
    public static class BankHelper
    {
        private static List<IUser> Users { get; set; } = new List<IUser>();
        private static int _lastAccountNumber = 53540000;
        private static decimal _interestRate = 0.05m;
        private static Dictionary<string, decimal> ExchangeRates = new Dictionary<string, decimal>
        {
            { "SEK", 1.0m },
            { "USD", 0.11m },
            { "EUR", 0.091m },
            { "GBP", 0.080m },
            { "BTC", 0.00000099m },
            { "DGC", 0.6m }
        };

        public static void AddUser(IUser user)
        {
            Users.Add(user);
        }

        public static void PrintUsers()
        {
            foreach (var user in Users.OfType<Customer>())
            {
                AnsiConsole.MarkupLine($"Name: [aquamarine1]{user.Name}[/], Email: [yellow]{user.Email}[/], UserName: [hotpink]{user.UserName}[/], Age: [salmon1]{user.Age}[/]");
            }

            AnsiConsole.MarkupLine("Press any key to continue");
            Console.ReadKey();
        }

        public static IUser? GetUserById(Guid id)
        {
            return Users.SingleOrDefault(u => u.Id == id);
        }

        public static List<Account>? GetAccounts(Guid userId, bool onlyChecking)
        {
            var user = Users.OfType<Customer>().SingleOrDefault(u => u.Id == userId);

            if (user == null || user.Accounts.Count == 0)
            {
                return null;
            }

            if (onlyChecking)
            {
                return user.Accounts.Where(a => a.GetType() == typeof(Account)).ToList();
            }
            else
            {
                return user.Accounts.ToList();
            }
        }

        public static int GenerateAccountNumber()
        {
            _lastAccountNumber++;
            return _lastAccountNumber;
        }

        public static IUser GetUserByLogin(string username, string password)
        {
            return Users.FirstOrDefault(u => (u.UserName.ToLower() == username.ToLower() || u.Email.ToLower() == username.ToLower()) && u.Password == password);
        }

        public static Account? GetAccountByNumber(int accountNumber)
        {
            foreach (var user in Users.OfType<Customer>())
            {
                var account = user.Accounts.SingleOrDefault(a => a.AccountNumber == accountNumber);
                if (account != null)
                    return account;
            }

            return null;
        }

        public static void UpdateExchangeRate(string key, decimal rate)
        {
            ExchangeRates[key] = rate;
        }

        public static decimal GetExchangeRateByKey(string key)
        {
            return ExchangeRates[key];
        }

        public static ICollection<string> GetExchangeRateKeys()
        {
            return ExchangeRates.Keys;
        }

        public static decimal CalculateExchange(decimal amount, string currentRateKey, string targetRateKey)
        {
            var targetRate = GetExchangeRateByKey(targetRateKey); // Get target exchange rate
            var amountInBaseRate = ConvertAmountToBaseRate(amount, currentRateKey); // Convert amount to base rate (SEK)

            return amountInBaseRate * targetRate; // Convert from base rate to target currency
        }

        private static decimal ConvertAmountToBaseRate(decimal amount, string currentRateKey)
        {
            if (currentRateKey == "SEK")
            {
                return amount;
            }

            var currentRate = GetExchangeRateByKey(currentRateKey);
            return amount / currentRate;
        }

        public static decimal GetInterestRate()
        {
            return _interestRate;
        }

        public static List<SavingsAccount> GetAllSavingsAccounts()
        {
            var savingsAccounts = new List<SavingsAccount>();
            foreach (var user in Users)
            {
                var userAccounts = GetAccounts(user.Id, false);
                if (userAccounts == null) continue;
                savingsAccounts.AddRange(userAccounts.OfType<SavingsAccount>()); // Fill SavingsAccount list from users accounts
            }

            return savingsAccounts;
        }
    }
}
