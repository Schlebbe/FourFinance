using FourFinance.Helpers;

namespace FourFinance
{
    internal class Program
    {
        public static readonly SemaphoreSlim ConsoleLock = new(1, 1);
        static async Task Main(string[] args)
        {
            var transactionScheduler = SchedulerHelper.StartTransactionSchedulerAsync(SchedulerHelper.cts.Token);
            var interestScheduler = SchedulerHelper.StartInterestSchedulerAsync(SchedulerHelper.cts.Token);
            DummyDataHelper.SeedDummyData(); // Seed some dummy users for testing
            LoginHelper.LoginPrompt();
            await transactionScheduler;
            await interestScheduler;
        }
    }
}
