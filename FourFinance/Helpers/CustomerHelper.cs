using FourFinance.Accounts;
using FourFinance.Users;
using Spectre.Console;

namespace FourFinance.Helpers
{
    public static class CustomerHelper
    {
        public static void Menu(Customer customer)
        {
            AnsiConsole.MarkupLine("Please choose an option from the menu below:\n");

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(3)
                    .AddChoices(new[]
                    {
                        "List accounts", "Open new account", "Logout", "Loan" //TODO: Add loan management option
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
                case "Loan":
                    Loan myLoan = new Loan(customer);
                    myLoan.CreateLoan();
                    return;
            }
        }

        private static void ListAccounts(Customer customer)
        {
            AnsiConsole.Clear();
            var accounts = BankHelper.GetAccounts(customer.Id);
            if (accounts == null || accounts.Count == 0)
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine("[yellow]You have no accounts. Please open a new account first.[/]");
                Menu(customer);
                return;
            }

            AnsiConsole.MarkupLine("Select an [blue]account[/] to manage:\n");

            var selectedAccount = AnsiConsole.Prompt(
                new SelectionPrompt<Account>()
                    .PageSize(5)
                    .UseConverter(a => $"Account number: {a.AccountNumber}\n  Balance: {a.GetBalance()} {a.GetCurrency()}\n")
                    .AddChoices(accounts));

            if (selectedAccount != null)
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine($"You selected account with number: [blue]{selectedAccount.AccountNumber}[/]");
                AnsiConsole.MarkupLine($"Current balance: {selectedAccount.GetBalance()} {selectedAccount.GetCurrency()}\n");

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .PageSize(3)
                        .AddChoices(new[] {
                                "Deposit", "Withdraw", "Transfer", "Return to menu"
                        }));

                switch (choice)
                {
                    case "Deposit":

                        break;
                    case "Withdraw":

                        break;
                    case "Transfer":

                        break;
                    case "Return to menu":
                        AnsiConsole.Clear();
                        Menu(customer);
                        break;
                }
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
            Menu(customer);
        }
    }
}