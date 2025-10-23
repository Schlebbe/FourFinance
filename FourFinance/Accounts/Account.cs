namespace FourFinance.Accounts
{
    public class Account
    {
        public Guid Id { get; set; }
        public int AccountNumber { get; set; }
        private decimal _balance;
        private Currency _currency;
        //public List<Loan> Loans { get; set; }
        //public List<Log> Logs { get; set; }

        public Account(int accountNumber)
        {
            Id = Guid.NewGuid();
            AccountNumber = accountNumber;
        }

        public void Withdraw(decimal amount)
        {
            if (amount > _balance)
            {
                Console.WriteLine("Insufficient funds.");
                return;
            }

            _balance -= amount;
            Console.WriteLine($"{amount} withdrawn. Current balance: {_balance}");
            //TODO: Log transaction
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Deposit amount must be positive.");
                return;
            }

            _balance += amount;
            Console.WriteLine($"{amount} deposited. Current balance: {_balance}");
            //TODO: Log transaction
        }

        public void Transfer(decimal amount, int accountNumber)
        {
            if (amount > _balance)
            {
                Console.WriteLine("Insufficient funds for transfer.");
                return;
            }

            _balance -= amount;
            //TODO: Find target account by accountNumber and deposit amount
            Console.WriteLine($"{amount} transfered to account {accountNumber}. Current balance: {_balance}");
            //TODO: Log transaction
        }

        public decimal GetBalance()
        {
            return _balance;
        }
    }
}
