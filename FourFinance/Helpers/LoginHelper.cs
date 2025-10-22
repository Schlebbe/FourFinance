using Spectre.Console;

namespace FourFinance.Helpers
{
    public static class LoginHelper
    {
        public static void LoginPrompt()
        {
            PrintAsciiWelcome();

            // Using Spectre.Console to create a simple login prompt
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(3)
                    .AddChoices(new[] {
                            "Login", "Exit"
                    }));

            AnsiConsole.Clear();

            if (choice == "Exit")
            {
                AnsiConsole.Markup($"Thank you for visiting [green]FourFinance[/]!");
                Environment.Exit(0);
            }

            Login();
        }

        public static void Login()
        {
            var username = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter [green]Username/Email[/]:"));
            var password = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter [red]Password[/]:")
                .Secret());

            var user = BankHelper.GetUserByLogin(username, password);

            if (user == null)
            {
                AnsiConsole.Markup("Invalid username/email or password. Please try again.");
                Login(); // Retry login
                return;
            }

            AnsiConsole.Markup($"Welcome back, [blue]{user.Name}[/]!");
        }

        private static void PrintAsciiWelcome()
        {
            AnsiConsole.Clear();

            var art = @"
       [blue]__        __   _  [/]                                      
       [blue]\ \      / /__| | ___ ___  _ __ ___   ___[/] 
        [blue]\ \ /\ / / _ \ |/ __/ _ \| '_ ` _ \ / _ \[/] 
         [blue]\ V  V /  __/ | (_| (_) | | | | | |  __/[/] 
          [blue]\_/\_/ \___|_|\___\___/|_| |_| |_|\___[/] 

          .---------------------------------------.      .----------------.
         /  ____________[deeppink4_2]BANK[/] OF [green]FOURFINANCE[/]______  \    /  .------------.  \
        /  /  ||  ||  ||  ||  ||  ||  ||  ||  ||  \  \  /  /  ________   \  \
       |  |   ||  ||  ||  ||  ||  ||  ||  ||  ||   |  | |  |  |[yellow]$$$$$$[/]| |  | |
       |  |   ||  ||  ||  ||  ||  ||  ||  ||  ||   |  | |  |  |[yellow]$$$$$$[/]| |  | |
       |  |   ||  ||  ||  ||  ||  ||  ||  ||  ||   |  | |  |  |[yellow]$$$$$$[/]| |  | |
       |  |   ||  ||  ||  ||  ||  ||  ||  ||  ||   |  | |  |  |[yellow]$$$$$$[/]| |  | |
       |  |   ||  ||  ||  ||  ||  ||  ||  ||  ||   |  | |  |  |[yellow]$$$$$$[/]| |  | |
        \  \______________________________________/  /   \  \  '------'  /  /
         '-------------------------------------------'     '--------------'
           [yellow]__ .--. _[/]
       [yellow].-;.-""-.-;`_;-,[/]
     [yellow].(_( `)-;___),-;_),[/]
    [yellow](.( `\.-._)-.(   ). )[/]
  ,[yellow](_`'--;.__\  _).;--'`_)[/]
 // [yellow])[/]`--..__ ``` _[yellow]( o )'('[/];.
 \;'        `````  `\\   '.\\
 /                   ':.___//
|                      '---'|
;                           ;
 \                         /
  '.                     .'
    '-,.__         __.,-'
     (___/`````````\___)
    ";

            // Render each line to allow Spectre.Console markup (colors) to apply properly.
            var lines = art.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                AnsiConsole.MarkupLine(line);
            }

            //AnsiConsole.WriteLine("");
        }
    }
}
