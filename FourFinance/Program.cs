using FourFinance.Helpers;

namespace FourFinance
{
    internal class Program
    {
        public static readonly SemaphoreSlim ConsoleLock = new(1, 1);
        private static readonly Task transactionScheduler = SchedulerHelper.StartTransactionSchedulerAsync(SchedulerHelper.cts.Token);
        private static readonly Task interestScheduler = SchedulerHelper.StartInterestSchedulerAsync(SchedulerHelper.cts.Token);
        static async Task Main(string[] args)
        {
            DummyDataHelper.SeedDummyData(); // Seed some dummy users for testing

            LoginHelper.LoginPrompt();

            // Wait for the schedulers to complete any remaining tasks
            await transactionScheduler;
            await interestScheduler;
        }
    }
}
