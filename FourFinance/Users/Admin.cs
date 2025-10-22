using FourFinance.Helpers;
using System.Transactions;

namespace FourFinance.Users
{
    public class Admin : IUser
    {
        public Guid Id { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }

        public Admin(Guid id, int age, string name, string email, string password, string userName)
        {
            Id = id;
            Age = age;
            Name = name;
            Email = email;
            Password = password;
            UserName = userName;
            Id = Guid.NewGuid();
        }

        public void AddCustomer()
        {
            //TODO: Add customer to management system list
            Console.WriteLine("Enter age of the customer:");
            var userInput = Console.ReadLine();

            if (string.IsNullOrEmpty(userInput) || !int.TryParse(userInput, out var age))
            {
                Console.WriteLine("Invalid age. Please enter a number.");
                return;
            }
           
            Console.WriteLine("Enter name of the customer:");
            var name = Console.ReadLine();

            if (string.IsNullOrEmpty(name)) 
            {
                Console.WriteLine("You need to write a name.");
                return; 
            }
           
            Console.WriteLine("Enter email to the customer:");
            var email = Console.ReadLine();

            if (string.IsNullOrEmpty(email)) 
            {
                Console.WriteLine("You need to write a email.");
                return; 
            }

            Console.WriteLine("Enter a password:");
            var password = Console.ReadLine();
            if (string.IsNullOrEmpty(password)) 
            {
                Console.WriteLine("You need to write a password.");
                return; 
            }

            Console.WriteLine("Enter a username:");
            var userName = Console.ReadLine();
            if (string.IsNullOrEmpty(userName)) 
            {
                Console.WriteLine("You need to write a username.");
                return;
            }

            var customer = new Customer(age, name, email, password, userName);
            BankHelper.AddUser(customer);
        }
    }
}
