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
                    .PageSize(4)
                    .AddChoices(new[]
                    {
                        "List accounts", "Open a checking account", "Open a savings account", "Request loan", "Logout"
                    }));

            switch (choice)
            {
                case "List accounts":
                    ListAccounts(customer);
                    return;
                case "Open a checking account":
                    OpenNewCheckingAccountMenu(customer);
                    return;
                case "Open a savings account":
                    OpenNewSavingsAccountMenu(customer);
                    return;
                case "Request loan":
                    var loan = new Loan();
                    loan.CreateLoan(customer);
                    return;
                case "Logout":
                    AnsiConsole.Clear();
                    AnsiConsole.Markup($"Thank you for banking with [green]FourFinance[/]!");
                    AnsiConsole.MarkupLine("Press any key to continue");
                    LoginHelper.LoginPrompt();
                    return;
            }
        }

        private async static void ListAccounts(Customer customer)
        {
            AnsiConsole.Clear();
            var accounts = BankHelper.GetAccounts(customer.Id, false);
            if (accounts == null || accounts.Count == 0)
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine("[yellow]You have no accounts. Please open a new account first.[/]");
                Menu(customer);
                return;
            }

            AnsiConsole.MarkupLine("Select an [blue]account[/] to manage:\n");

            Account? selectedAccount = null;
            var exitAccount = new Account(000, "NaN");
            var choices = accounts.Concat(new[] { exitAccount }).ToList();

            await Program.ConsoleLock.WaitAsync(); // Ensure exclusive access to the console
            try
            {
                // Selection prompt for accounts including an exit option
                selectedAccount = AnsiConsole.Prompt(
                    new SelectionPrompt<Account>()
                        .PageSize(5)
                        .UseConverter(a => // Converter to display account details
                            a.AccountNumber == 000 // Check if the selected account is the dummy account exit option
                                ? "[red]Exit[/]"
                                : $"{(a.GetType() == typeof(Account) ? "Checking" : "Savings")} account: {a.AccountNumber}\n  Balance: {a.GetBalance():F2} {a.GetCurrency()}\n")
                        .AddChoices(choices));

                if (selectedAccount.AccountNumber == 000)
                {
                    AnsiConsole.Clear();
                    Menu(customer);
                    return;
                }
            }
            finally
            {
                Program.ConsoleLock.Release(); // Release the console lock
            }

            if (selectedAccount != null)
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine($"You selected account with number: [blue]{selectedAccount.AccountNumber}[/]");
                AnsiConsole.MarkupLine($"Current balance: [green]{selectedAccount.GetBalance():F2} {selectedAccount.GetCurrency()}[/]\n");

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .PageSize(3)
                        .AddChoices(new[] {
                                "Deposit", "Withdraw", "Transfer", "History", "Return to menu"
                        }));

                switch (choice)
                {
                    case "Deposit":
                        HandleDeposit(selectedAccount);
                        AnsiConsole.MarkupLine("Press any key to continue");
                        Console.ReadKey();
                        ListAccounts(customer);
                        break;
                    case "Withdraw":
                        HandleWithdrawal(selectedAccount);
                        AnsiConsole.MarkupLine("Press any key to continue");
                        Console.ReadKey();
                        ListAccounts(customer);
                        break;
                    case "Transfer":
                        HandleTransfer(customer, selectedAccount);
                        ListAccounts(customer);
                        break;
                    case "History":
                        AnsiConsole.Clear();
                        selectedAccount.printLogs();
                        ListAccounts(customer);
                        break;
                    case "Return to menu":
                        AnsiConsole.Clear();
                        Menu(customer);
                        break;
                }
            }
            return;
        }

        private static async Task HandleTransfer(Customer customer, Account selectedAccount)
        {
            decimal amount = AnsiConsole.Ask<decimal>("Enter amount to transfer: ");
            int targetAccountNumber = AnsiConsole.Ask<int>("Enter account number: ");

            if (amount <= 0)
            {
                AnsiConsole.MarkupLine("[red]Amount must be greater than zero.[/]");
                ListAccounts(customer);
                return;
            }

            if (targetAccountNumber <= 0)
            {
                AnsiConsole.MarkupLine("[red]Invalid account number.[/]");
                ListAccounts(customer);
                return;
            }

            if (targetAccountNumber == selectedAccount.AccountNumber)
            {
                AnsiConsole.MarkupLine("[red]You cannot transfer to the same account.[/]");
                ListAccounts(customer);
                return;
            }

            // Enqueue the transfer operation to be processed by the scheduler using lambda expression
            SchedulerHelper.pendingTransactions.Enqueue(() =>
            {
                var result = selectedAccount.Transfer(amount, targetAccountNumber);

                if (result == true)
                {
                    AnsiConsole.MarkupLine($"New balance: [green]{selectedAccount.GetBalance():F2} {selectedAccount.GetCurrency()}[/]");
                }
                return Task.CompletedTask;
            });
        }

        private static void HandleDeposit(Account selectedAccount)
        {
            decimal amount = AnsiConsole.Ask<decimal>("Enter amount to deposit: ");

            if (amount <= 0)
            {
                AnsiConsole.MarkupLine("[red]Amount must be greater than zero. [/]");
                return;
            }
            var result = selectedAccount.Deposit(amount, true, false); // Deposit to selected account with printing enabled and interest disabled

            if (result == true)
            {
                AnsiConsole.MarkupLine($"[green]{amount:F2} [/]deposited. Current [blue]balance[/]: [green]{selectedAccount.GetBalance():F2}[/] {selectedAccount.GetCurrency()}");
            }
        }

        private static void HandleWithdrawal(Account selectedAccount)
        {
            decimal amount = AnsiConsole.Ask<decimal>("Enter amount to withdraw: ");

            if (amount <= 0)
            {
                AnsiConsole.MarkupLine("[red]Amount must be greater than zero. [/]");
                return;
            }
            var result = selectedAccount.Withdraw(amount);

            if (result == true)
            {
                AnsiConsole.MarkupLine($"[green]{amount:F2} [/]withdrawn. Current [blue]balance[/]: [green]{selectedAccount.GetBalance():F2}[/] {selectedAccount.GetCurrency()}");
            }
        }

        private static void OpenNewCheckingAccountMenu(Customer customer)
        {
            AnsiConsole.MarkupLine("To create a new [green]checking account[/], [yellow]please[/] provide the following details:");

            // Present Currency key values as selectable choices and return a Currency key string
            var currency = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What [blue]currency[/] would you like to use?")
                    .PageSize(10)
                    .AddChoices(BankHelper.GetExchangeRateKeys()));

            AnsiConsole.Clear();
            customer.CreateAccount(currency, false); // Create checking account

            // Return the flow to the start of the customer menu
            Menu(customer);
        }

        private static void OpenNewSavingsAccountMenu(Customer customer)
        {
            AnsiConsole.MarkupLine("To create a new [green]savings account[/], [yellow]please[/] provide the following details:");

            // Present Currency key values as selectable choices and return a Currency key string
            var currency = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What [blue]currency[/] would you like to use?")
                    .PageSize(10)
                    .AddChoices(BankHelper.GetExchangeRateKeys()));

            AnsiConsole.Clear();
            customer.CreateAccount(currency, true); // Create savings account

            // Return the flow to the start of the customer menu
            Menu(customer);
        }
    }
}