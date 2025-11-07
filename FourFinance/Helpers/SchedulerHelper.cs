using Spectre.Console;
using System;
using System.Collections.Concurrent;

namespace FourFinance.Helpers
{
    public static class SchedulerHelper
    {
        public static readonly ConcurrentQueue<Func<Task>> pendingTransactions = new ConcurrentQueue<Func<Task>>(); // Queue of pending transaction methods
        public static CancellationTokenSource cts = new CancellationTokenSource(); // Cancellation token for stopping the schedulers

        // Transaction scheduler that processes pending transactions every 15 minutes
        public static async Task StartTransactionSchedulerAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(15), token);

                    while (pendingTransactions.TryDequeue(out var action)) // Process all pending transactions
                    {
                        try
                        {
                            await Program.ConsoleLock.WaitAsync(token); // Ensure exclusive access to the console
                            await action(); // Execute the transaction method
                        }
                        catch (Exception ex)
                        {
                            AnsiConsole.MarkupLine($"Error running action: {ex.Message}");
                        }
                        finally
                        {
                            Program.ConsoleLock.Release(); // Release the console lock
                        }
                    }
                }
            }
            catch (TaskCanceledException)
            {
                // just ignore, normal shutdown
            }
        }

        // Interest scheduler that calculates and applies interest to all savings accounts every 15 seconds
        public static async Task StartInterestSchedulerAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(15), token);

                    try
                    {
                        var savingsAccounts = BankHelper.GetAllSavingsAccounts();
                        foreach (var account in savingsAccounts)
                        {
                            account.CalculateAndApplyInterest(); // Calculate and apply interest to each savings account
                        }
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine($"Error calculating interest: {ex.Message}");
                    }
                }
            }
            catch (TaskCanceledException)
            {
                // just ignore, normal shutdown
            }
        }
    }
}
