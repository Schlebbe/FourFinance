using Spectre.Console;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourFinance.Helpers
{
    public static class TransactionHelper
    {
        public static readonly ConcurrentQueue<Func<Task>> pendingActions = new ConcurrentQueue<Func<Task>>();
        public static CancellationTokenSource cts = new CancellationTokenSource();

        public static async Task StartSchedulerAsync(CancellationToken token) 
        {
            while (!token.IsCancellationRequested) 
            {
                await Task.Delay(TimeSpan.FromMinutes(15), token);

                while (pendingActions.TryDequeue(out var action)) 
                {
                    try
                    {
                        await action();
                    }
                    catch (Exception ex) 
                    {
                        AnsiConsole.MarkupLine($"Error running action: {ex.Message}");
                    }
                }
            }
        }
    }
}
