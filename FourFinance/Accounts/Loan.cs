using FourFinance.Helpers;
using FourFinance.Users;
using Spectre.Console;

namespace FourFinance.Accounts;

public class Loan
{
    public Guid Id { get; set; }
    public decimal InterestRate { get; set; }

    public Loan()
    {
        Id = Guid.NewGuid();
        InterestRate = 0.05m;
    }

    public void CreateLoan(Customer customer)
    {
        AnsiConsole.Clear();
        decimal maxLoanAmount = (customer.CustomerAssets() - customer.ActiveLoanAmount) * 5;

        var accounts = BankHelper.GetAccounts(customer.Id);

        if (accounts == null || accounts.Count == 0)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[yellow]You have no accounts. Please open a new [blue]account[/] first.[/]");
            CustomerHelper.Menu(customer);
            return;
        }
        else if (maxLoanAmount <= 0)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[red]Not enough [green]assets[/] on your [blue]accounts[/]. [orchid1]Please[/] deposit to an [blue]account[/] first.[/]");
            CustomerHelper.Menu(customer);
            return;
        }

        AnsiConsole.MarkupLine("Select an [blue]account[/] to add a [green]loan[/] to:\n");

        var selectedAccount = AnsiConsole.Prompt(
            new SelectionPrompt<Account>()
                .PageSize(5)
                .UseConverter(a =>
                    $"Account number: {a.AccountNumber}\n  Balance: {a.GetBalance()} {a.GetCurrency()}\n")
                .AddChoices(accounts));

        AnsiConsole.Clear();
        AnsiConsole.Markup($"Desired [green]amount[/]: ");
        bool success = decimal.TryParse(Console.ReadLine(), out decimal principal);
        if (success != true)
        {
            AnsiConsole.MarkupLine("[red]Incorrect input, please try again by pressing any button..[/]");
            Console.ReadKey();
            CreateLoan(customer);
            return;
        }

        if (principal < maxLoanAmount && principal > 0)
        {
            AnsiConsole.MarkupLine($"[green]{principal} {selectedAccount.GetCurrency()}[/] with an [salmon1]interest[/] of [salmon1]{(int)(InterestRate * 100m)}%[/] will be [green]{principal * (1 + InterestRate)} {selectedAccount.GetCurrency()}[/]");

            if (selectedAccount != null)
            {
                AnsiConsole.MarkupLine($"You selected [blue]account[/] with number: [blue]{selectedAccount.AccountNumber}[/]");
                AnsiConsole.MarkupLine($"Previous balance: [green]{selectedAccount.GetBalance()} {selectedAccount.GetCurrency()}[/]\n");
                customer.ActiveLoanAmount += principal;
                selectedAccount.Deposit(principal);
                AnsiConsole.MarkupLine($"New balance: [green]{selectedAccount.GetBalance()} {selectedAccount.GetCurrency()}[/]\n");
                CustomerHelper.Menu(customer);
            }
        }
        else
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[red]Invalid amount.\nPress any key to continue...[/]");
            Console.ReadKey();
            CreateLoan(customer);
        }
    }
}