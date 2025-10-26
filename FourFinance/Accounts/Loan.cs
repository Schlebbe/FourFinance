using FourFinance.Helpers;
using FourFinance.Users;

namespace FourFinance.Accounts;

public class Loan
{
    private decimal Principal { get; set; }
    private decimal Interest { get; set; }
    private decimal LoanAmount { get; set; }
    private Customer Customer { get;}
    private Account AccountNumber;

    public Loan(decimal principal, decimal interest, Account accountNumber)
    {
        AccountNumber = accountNumber;
        Principal = principal;
        Interest = interest;
    }

    public void CreateLoan(decimal principal, decimal interest, decimal loanAmount, Account accountNumber)
    {
        //TODO: add a way to create a loan
        foreach (var Account in BankHelper.GetAccounts(Customer.Id))
        {
            Console.WriteLine($"Account:{accountNumber}\nBalance: {Account.GetBalance()}\n\n");
        }

    }

    public decimal CalculateInterest(decimal principal, decimal interest)
    {
        decimal loanAmount = principal * interest;
        return loanAmount;
    }
}