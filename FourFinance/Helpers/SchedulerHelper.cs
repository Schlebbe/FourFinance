using Spectre.Console;
using System;
using System.Collections.Concurrent;

namespace FourFinance.Helpers
{
    public static class SchedulerHelper
    {
        public static readonly ConcurrentQueue<Func<Task>> pendingTransactions = new ConcurrentQueue<Func<Task>>();
        public static CancellationTokenSource cts = new CancellationTokenSource();

        public static async Task StartTransactionSchedulerAsync(CancellationToken token) 
        {
            while (!token.IsCancellationRequested) 
            {
                await Task.Delay(TimeSpan.FromMinutes(15), token);

                while (pendingTransactions.TryDequeue(out var action)) 
                {
                    try
                    {
                        await Program.ConsoleLock.WaitAsync(token);
                        await action();
                    }
                    catch (Exception ex) 
                    {
                        AnsiConsole.MarkupLine($"Error running action: {ex.Message}");
                    }
                    finally
                    {
                        Program.ConsoleLock.Release();
                    }
                }
            }
        }

        public static async Task StartInterestSchedulerAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(15), token);

                try
                {
                    await Program.ConsoleLock.WaitAsync(token);
                    var savingsAccounts = BankHelper.GetAllSavingsAccounts();
                    foreach (var account in savingsAccounts)
                    {
                        account.CalculateAndApplyInterest();
                    }
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"Error calculating interest: {ex.Message}");
                }
                finally
                {
                    Program.ConsoleLock.Release();
                }
            }
        }
    }
}
