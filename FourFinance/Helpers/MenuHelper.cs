using FourFinance.Accounts;
using FourFinance.Users;
using Spectre.Console;

namespace FourFinance.Helpers
{
    public static class MenuHelper
    {
        public static void CustomerMenu(Customer customer)
        {
            AnsiConsole.MarkupLine("Please choose an option from the menu below:");
            AnsiConsole.WriteLine();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(3)
                    .AddChoices(new[] {
                            "List accounts", "Open new account", "Logout"
                    }));

            switch (choice)
            {
                case "List accounts":
                    ListAccounts(customer);
                    break;
                case "Open new account":
                    OpenNewAccountMenu(customer);
                    return;
                case "Logout":
                    AnsiConsole.Clear();
                    AnsiConsole.Markup($"Thank you for banking with [green]FourFinance[/]!");
                    Environment.Exit(0);
                    break;
            }
        }

        private static void ListAccounts(Customer customer)
        {
            var accounts = BankHelper.GetAccounts(customer.Id);
            if (accounts == null || accounts.Count == 0)
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine("[yellow]You have no accounts. Please open a new account first.[/]");
                CustomerMenu(customer);
                return;
            }

            var selectedAccount = AnsiConsole.Prompt(
                new SelectionPrompt<Account>()
                    .PageSize(5)
                    .UseConverter(a => $"Account number: {a.AccountNumber}\n  Balance: {a.GetBalance()} {a.GetCurrency()}\n")
                    .AddChoices(accounts));

            if (selectedAccount != null)
            {
                AnsiConsole.Clear();
                //TODO: Add AccountMenu method to manage selected account
            }

            return;
        }

        private static void OpenNewAccountMenu(Customer customer)
        {
            AnsiConsole.MarkupLine("To create a new [green]account[/], [yellow]please[/] provide the following details:");

            // Present Currency enum values as selectable choices and return a Currency value
            var currency = AnsiConsole.Prompt(
                new SelectionPrompt<Currency>()
                    .Title("What [blue]currency[/] would you like to use?")
                    .PageSize(10)
                    .AddChoices(Enum.GetValues<Currency>()));

            AnsiConsole.Clear();
            customer.CreateAccount(currency);

            // Return the flow to the start of the customer menu
            CustomerMenu(customer);
        }

        public static void AdminMenu(Admin admin)
        {

            throw new NotImplementedException();
        }
    }
}