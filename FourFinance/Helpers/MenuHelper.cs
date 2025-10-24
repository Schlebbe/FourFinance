using FourFinance.Users;
using Spectre.Console;

namespace FourFinance.Helpers
{
    public static class MenuHelper
    {
        public static void CustomerMenu(Customer customer)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(3)
                    .AddChoices(new[] {
                            "List accounts", "Open new account", "Logout"
                    }));

            switch (choice)
            {
                case "List accounts":
                    //BankHelper.GetAccounts(customer.Id); //TODO: Add account listing
                    //TODO: After listing accounts, allow user to select an account to manage
                    //New method? AccountMenu(accounts, customer);?
                    break;
                case "Open new account":
                    //customer.CreateAccount(); //TODO: Implement account creation flow
                    //TODO: Refresh menu after account creation
                    //CustomerMenu(customer); //Like this?
                    break;
                case "Logout":
                    AnsiConsole.Clear();
                    AnsiConsole.Markup($"Thank you for banking with [green]FourFinance[/]!");
                    Environment.Exit(0);
                    break;
            }

            throw new NotImplementedException();
        }

        public static void AdminMenu(Admin admin)
        {

            throw new NotImplementedException();
        }
    }
}