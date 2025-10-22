using Spectre.Console;

namespace FourFinance.Helpers
{
    public static class LoginHelper
    {
        public static void LoginPrompt()
        {
            // Using Spectre.Console to create a simple login prompt
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Welcome to [green]FourFinance[/]")
                    .PageSize(3)
                    .AddChoices(new[] {
                        "Login", "Exit"
                    }));

            if (choice == "Exit")
            {
                AnsiConsole.WriteLine($"Thank you for visiting [green]FourFinance[/]!");
                Environment.Exit(0);
            }

            Login();
        }

        public static void Login()
        {
            var username = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter [green]Username/Email[/]:"));
            var password = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter [red]Password[/]:")
                .Secret());

            var user = BankHelper.GetUserByLogin(username, password);

            if (user == null)
            {
                AnsiConsole.WriteLine("Invalid username/email or password. Please try again.");
                Login(); // Retry login
                return;
            }

            AnsiConsole.WriteLine($"Welcome back, {user.Name}!");
        }
    }
}
