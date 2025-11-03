using FourFinance.Accounts;
using FourFinance.Users;
using Spectre.Console;

namespace FourFinance.Helpers
{
    public static class BankHelper
    {
        private static List<IUser> Users { get; set; } = new List<IUser>();
        private static int _lastAccountNumber = 53540000;
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

        public static void GetUsers()
        {
            foreach (var user in Users)
            {
                AnsiConsole.MarkupLine($"Name: {user.Name}, Email: {user.Email}, UserName: {user.UserName}, Age: {user.Age} IsAdmin: {user.GetType() == typeof(Admin)}");
            }
        }

        public static IUser? GetUserById(Guid id)
        {
            return Users.FirstOrDefault(u => u.Id == id);
        }
        
        public static List<Account>? GetAccounts(Guid userId)
        {
            var user = Users.OfType<Customer>().FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                //AnsiConsole.MarkupLine($"No user found with ID {userId}");
                return null;
            }

            if (user.Accounts.Count == 0)
            {
                //AnsiConsole.MarkupLine($"{user.Name} has no accounts.");
                return null;
            }
            return user.Accounts;
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
                var account = user.Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
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
            var targetRate = GetExchangeRateByKey(targetRateKey);
            var amountInBaseRate = ConvertAmountToBaseRate(amount, currentRateKey);

            return amountInBaseRate * targetRate;
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
    }
}
