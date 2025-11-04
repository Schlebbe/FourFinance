using FourFinance.Helpers;
using System.Threading.Tasks;

namespace FourFinance
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            var taskScheduler = TransactionHelper.StartSchedulerAsync(cts.Token);
            DummyDataHelper.SeedDummyData(); // Seed some dummy users for testing
            LoginHelper.LoginPrompt();
            await taskScheduler;
        }
    }
}
