using FourFinance.Accounts;
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
    }
}
