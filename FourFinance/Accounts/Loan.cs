using FourFinance.Helpers;
using FourFinance.Users;
using Spectre.Console;

namespace FourFinance.Accounts;

public class Loan
{
    private decimal Principal { get; set; }
    private decimal Interest { get; set; }
    private decimal LoanAmount { get; set; }
    private Customer Customer { get; }
    private Account AccountNumber;
    
    public Loan(Customer customer)
    {
        Customer = customer ?? throw new ArgumentNullException(nameof(customer));
    }

    public void CreateLoan()
    {
        decimal principal = 0;
        decimal interest = 0.05m;
        decimal loanAmount = 0;
        
        Console.Clear();
        Console.Write("Desired amount:");
        bool success = decimal.TryParse(Console.ReadLine(), out principal);
        decimal maxLoanAmount = Customer.CustomerAssets() - Customer.ActiveLoanAmount;
        if (success != true)
        {
            Console.WriteLine("Incorrect input, please try again by pressing any button..");
            Console.ReadKey();
            CreateLoan();
        }

        if (principal < maxLoanAmount && principal > 0)
        {
            Console.WriteLine($"{principal} with an interest rate of {interest} will be {principal + (1 + interest)}");

            var selectedAccount = AnsiConsole.Prompt(
                new SelectionPrompt<Account>()
                    .PageSize(5)
                    .UseConverter(a =>
                        $"Account number: {a.AccountNumber}\n  Balance: {a.GetBalance()} {a.GetCurrency()}\n")
                    .AddChoices());

            if (selectedAccount != null)
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine($"You selected account with number: [blue]{selectedAccount.AccountNumber}[/]");
                AnsiConsole.MarkupLine($"Current balance: {selectedAccount.GetBalance()} {selectedAccount.GetCurrency()}\n");
                selectedAccount.Deposit(principal);
                AnsiConsole.MarkupLine($"New balance: {selectedAccount.GetBalance()} {selectedAccount.GetCurrency()}\n");
            }

        }
        else
        {
            Console.Clear();
            Console.WriteLine("Invalid amount.\nPress any key to continue...");
            Console.ReadKey();
            CreateLoan();
        }
    }
    
    decimal CalculateInterest(decimal principal, decimal interest)
    {
        decimal loanAmount = principal * interest;
        return loanAmount;
    }
}