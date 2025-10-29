using FourFinance.Users;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourFinance.Helpers
{
    internal static class AdminHelper
    {
        public static void Menu(Admin admin)
        {
            Console.Clear();

            AnsiConsole.MarkupLine("Please choose an option from the menu below:\n");

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(3)
                    .AddChoices(new[] {
                            "Add customer", "Logout" //TODO: Remove customer.
                    }));

            switch (choice)
            {
                case "Add customer":
                    admin.AddCustomer();
                    Menu(admin);
                    break;
                case "Logout":
                    AnsiConsole.Clear();
                    AnsiConsole.Markup($"Thank you for banking with [green]FourFinance[/]!");
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
