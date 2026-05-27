namespace BudgetTracker.Application.Wallets.Exceptions;

public sealed class ConcurrencyConflictException : Exception
{
    public Guid WalletId { get; }
    
    public ConcurrencyConflictException(Guid walletId, Exception? innerException = null) 
        : base($"Wallet {walletId} was modified by another operation.", innerException)
    {
        WalletId = walletId;
    }
}