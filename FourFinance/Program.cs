using FourFinance.Helpers;

namespace FourFinance
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DummyDataHelper.SeedDummyData(); // Seed some dummy users for testing
            LoginHelper.LoginPrompt();
        }

    }
}
