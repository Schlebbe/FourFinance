using FourFinance.Helpers;
using Spectre.Console;
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
            Console.Clear();
            AnsiConsole.MarkupLine("Enter [blue]age[/] of the [green]customer[/]:");
            var userInput = Console.ReadLine();

            if (string.IsNullOrEmpty(userInput) || !int.TryParse(userInput, out var age))
            {
                AnsiConsole.MarkupLine("[red]Invalid[/] [blue]age.[/] Please enter a number.");
                AnsiConsole.MarkupLine("Press any key to continue");
                Console.ReadKey();
                AddCustomer();
                return;
            }
           
            AnsiConsole.MarkupLine("Enter [blue]name[/] of the [green]customer[/]:");
            var name = Console.ReadLine();

            if (string.IsNullOrEmpty(name)) 
            {
                AnsiConsole.MarkupLine("[red]You need to write a[/] [blue]name.[/]");
                AnsiConsole.MarkupLine("Press any key to continue");
                Console.ReadKey();
                AddCustomer();
                return; 
            }
           
            AnsiConsole.MarkupLine("Enter [blue]email[/] to the [green]customer[/]:");
            var email = Console.ReadLine();

            if (string.IsNullOrEmpty(email)) 
            {
                AnsiConsole.MarkupLine("[red]You need to write a[/] [blue]email.[/]");
                AnsiConsole.MarkupLine("Press any key to continue");
                Console.ReadKey();
                AddCustomer();
                return; 
            }

            AnsiConsole.MarkupLine("Enter a [red]password:[/]");
            var password = Console.ReadLine();
            if (string.IsNullOrEmpty(password)) 
            {
                AnsiConsole.MarkupLine("[red]You need to write a password.[/]");
                AnsiConsole.MarkupLine("Press any key to continue");
                Console.ReadKey();
                AddCustomer();
                return; 
            }

            AnsiConsole.MarkupLine("Enter a [blue]username:[/]");
            var userName = Console.ReadLine();
            if (string.IsNullOrEmpty(userName)) 
            {
                AnsiConsole.MarkupLine("[red]You need to write a username.[/]");
                AnsiConsole.MarkupLine("Press any key to continue");
                Console.ReadKey();
                AddCustomer();
                return;
            }

            var customer = new Customer(age, name, email, password, userName);
            BankHelper.AddUser(customer);
            AnsiConsole.MarkupLine($"Added [green]customer:[/] [blue]{customer.Name}[/]");
            AnsiConsole.MarkupLine("Press any key to return to menu.");
            Console.ReadKey();
        }
    }
}
