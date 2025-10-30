﻿using FourFinance.Accounts;
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
                    .AddChoices(new[] {
                            "List accounts", "Open new account", "Logout" //TODO: Add loan management option
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
                        AnsiConsole.MarkupLine("Press any key to continue");
                        Console.ReadKey();
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

        private static void HandleTransfer(Customer customer, Account selectedAccount)
        {
            decimal amount = AnsiConsole.Ask<decimal>("Enter amount to transfer: ");
            int targetAccountNumber = AnsiConsole.Ask<int>("Enter account number: ");

            if (amount <= 0)
            {
                AnsiConsole.MarkupLine("[red]Amount must be greater than zero. [/]");
                return;
            }
            var result = selectedAccount.Transfer(amount, targetAccountNumber, customer);

            if (result == true)
            {
                AnsiConsole.MarkupLine($"New balance: {selectedAccount.GetBalance()} {selectedAccount.GetCurrency()}");
                return;
            }
        }

        private static void HandleDeposit(Account selectedAccount)
        {
            decimal amount = AnsiConsole.Ask<decimal>("Enter amount to deposit: ");

            if (amount <= 0)
            {
                AnsiConsole.MarkupLine("[red]Amount must be greater than zero. [/]");
                return;
            }
            var result = selectedAccount.Deposit(amount);

            if (result == true)
            {
                AnsiConsole.MarkupLine($"[green]{amount} [/]deposited. Current [blue]balance[/]: [green]{selectedAccount.GetBalance()}[/] {selectedAccount.GetCurrency()}");
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
                AnsiConsole.MarkupLine($"[green]{amount} [/]withdrawn. Current [blue]balance[/]: [green]{selectedAccount.GetBalance()}[/] {selectedAccount.GetCurrency()}");
            }
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