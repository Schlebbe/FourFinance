using FourFinance.Users;
using Spectre.Console;

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

        public static int GetAccount(int accountNumber)
        {
            //TODO: implement account retrieval
            throw new NotImplementedException();
        }

        public static int GenerateAccountNumber()
        {
            _lastAccountNumber++;
            return _lastAccountNumber;
        }

        public static IUser GetUserByLogin(string username, string password)
        {
            return Users.FirstOrDefault(u => (u.UserName == username || u.Email == username) && u.Password == password);
        }
    }
}
