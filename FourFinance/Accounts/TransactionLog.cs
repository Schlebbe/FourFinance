namespace FourFinance.Accounts;

public class TransactionLog
{
    public DateTime Timestamp { get; set; }
    public string Type { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int AccountNumber { get; set; }
    public int? TargetAccountNumber { get; set; } // For transfers
    public string Currency { get; set; } = string.Empty;
    public decimal BalanceAfter { get; set; }
    public string Message { get; set; } = string.Empty;
    
    public override string ToString()
    {
        if (TargetAccountNumber.HasValue)
            return $"{Timestamp:G} | {Type}: {Amount} {Currency} from {AccountNumber} → {TargetAccountNumber} | Balance: {BalanceAfter} | {Message}";
        else
            return $"{Timestamp:G} | {Type}: {Amount} {Currency} | Account: {AccountNumber} | Balance: {BalanceAfter} | {Message}";
    }
}