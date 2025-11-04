using FourFinance.Helpers;
using System.Threading.Tasks;

namespace FourFinance
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var taskScheduler = TransactionHelper.StartSchedulerAsync(TransactionHelper.cts.Token);
            DummyDataHelper.SeedDummyData(); // Seed some dummy users for testing
            LoginHelper.LoginPrompt();
            await taskScheduler;
        }
    }
}
