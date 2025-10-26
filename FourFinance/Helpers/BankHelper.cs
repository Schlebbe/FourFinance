using FourFinance.Accounts;
using FourFinance.Users;

namespace FourFinance.Helpers
{
    public static class BankHelper
    {
        private static List<IUser> Users { get; set; } = new List<IUser>();
        private static int _lastAccountNumber = 53540000;

        public static void AddUser(IUser user)
        {
            Users.Add(user);
        }

        public static void GetUsers()
        {
            foreach (var user in Users)
            {
                Console.WriteLine($"Name: {user.Name}, Email: {user.Email}, UserName: {user.UserName}, Age: {user.Age} IsAdmin: {user.GetType() == typeof(Admin)}");
            }
        }

        public static IUser? GetUserById(Guid id)
        {
            return Users.FirstOrDefault(u => u.Id == id);
        }
        
        public static List<Account> GetAccounts(Guid userId)
        {
            var user = Users.OfType<Customer>().FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                Console.WriteLine($"No user found with ID {userId}");
                return null;
            }

            if (user.Accounts.Count == 0)
            {
                Console.WriteLine($"{user.Name} has no accounts.");
                return new List<Account>();
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
    }
}
