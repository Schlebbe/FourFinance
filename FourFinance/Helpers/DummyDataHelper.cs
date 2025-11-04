using FourFinance.Users;

namespace FourFinance.Helpers
{
    public static class DummyDataHelper
    {
        public static void SeedDummyData()
        {
            BankHelper.AddUser(new Admin(23, "Admin", "AdminUser@example.com", "a", "A"));
            BankHelper.AddUser(new Admin(82, "Admin Too", "AdminToo@example.com", "adminpass", "AdminToo"));
            BankHelper.AddUser(new Admin(32, "Admin Also", "AdminAlso@example.com", "adminpass", "AdminAlso"));
            BankHelper.AddUser(new Admin(46, "Admin Adminsson", "AdminAdminsson@example.com", "adminpass", "AdminAdminsson"));
            BankHelper.AddUser(new Admin(30, "Admin Adminsdotter", "AdminAdminsdotter@example.com", "adminpass", "AdminAdminsdotter"));

            BankHelper.AddUser(new Customer(25, "John Doe", "JohnDoe@example.com", "password123", "JohnDoe"));
            BankHelper.AddUser(new Customer(40, "Jane Smith", "JaneSmith@example.com", "securepass", "JaneSmith"));
            BankHelper.AddUser(new Customer(30, "Alice Johnson", "AliceJohnson@example.com", "alicepass", "AliceJohnson"));
            BankHelper.AddUser(new Customer(28, "Bob Brown", "BobBrown@example.com", "b", "b"));
            BankHelper.AddUser(new Customer(35, "Charlie Davis", "CharlieDavis@example.com", "charlie123", "CharlieDavis"));
        }
    }
}
