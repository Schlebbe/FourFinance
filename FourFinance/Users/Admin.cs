using FourFinance.Helpers;

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

        public Admin(int age, string name, string email, string password, string userName)
        {
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
            //var age = Console.ReadLine();
            //var customer = new Customer();
            //BankHelper.AddUser(customer);
        }
    }
}
