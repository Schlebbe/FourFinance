using FourFinance.Helpers;
using FourFinance.Users;
using Spectre.Console;

namespace FourFinance.Accounts;

public class Loan
{
    public Guid Id { get; set; }
    public decimal InterestRate { get; set; }
    public decimal Principal { get; set; }

    public Loan()
    {
        Id = Guid.NewGuid();
        InterestRate = 0.05m;
    }

    public void CreateLoan(Customer customer)
    {
        AnsiConsole.Clear();
        var accounts = BankHelper.GetAccounts(customer.Id, onlyChecking: true);
        var assets = customer.CustomerAssets();

        if (accounts == null || accounts.Count == 0)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[yellow]You have no accounts. Please open a new [blue]account[/] first.[/]");
            CustomerHelper.Menu(customer);
            return;
        }

        AnsiConsole.MarkupLine("Select an [blue]account[/] to add a [green]loan[/] to:\n");

        decimal maxLoanAmount = assets * 5;

        if (maxLoanAmount <= 0)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[red]Not enough [green]assets[/] on your [blue]accounts[/]. [orchid1]Please[/] deposit to an [blue]account[/] first.[/]");
            CustomerHelper.Menu(customer);
            return;
        }

        // Using Spectre.Console to create a selection prompt for accounts
        var selectedAccount = AnsiConsole.Prompt(
            new SelectionPrompt<Account>()
                .PageSize(5)
                .UseConverter(a =>
                    $"Account number: {a.AccountNumber}\n  Balance: {a.GetBalance():F2} {a.GetCurrency()}\n")
                .AddChoices(accounts));

        var remainingLoanAmount = maxLoanAmount - selectedAccount.GetActiveLoanAmount();

        AnsiConsole.Clear();
        Principal = AnsiConsole.Prompt(
            new TextPrompt<decimal>("Desired [green]amount[/]: ")
                .ValidationErrorMessage("[red]Incorrect input, please try again by pressing any button..[/]")
        );

        if (remainingLoanAmount < Principal)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine($"[red][green]{remainingLoanAmount:F2} {selectedAccount.GetCurrency()}[/] available to loan. Desired [green]loan[/] amount: [green]{Principal} {selectedAccount.GetCurrency()}[/][/]");
            CustomerHelper.Menu(customer);
            return;
        }

        // Validate loan amount
        if (Principal <= maxLoanAmount && Principal > 0)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine($"[green]{Principal:F2} {selectedAccount.GetCurrency()}[/] with an [salmon1]interest[/] of [salmon1]{(int)(InterestRate * 100m)}%[/] will be [green]{Principal * (1 + InterestRate)} {selectedAccount.GetCurrency()}[/]");
            
            if (selectedAccount != null)
            {
                // Deposit the loan amount into the selected account
                AnsiConsole.MarkupLine($"You selected [blue]account[/] with number: [blue]{selectedAccount.AccountNumber}[/]");
                AnsiConsole.MarkupLine($"Previous balance: [green]{selectedAccount.GetBalance():F2} {selectedAccount.GetCurrency()}[/]\n");
                selectedAccount.Deposit(Principal, false, false);
                selectedAccount.Loans.Add(this); // Add the loan to the account's loan list
                AnsiConsole.MarkupLine($"New balance: [green]{selectedAccount.GetBalance():F2} {selectedAccount.GetCurrency()}[/]\n");
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