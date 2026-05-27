using BudgetTracker.Shared;

namespace BudgetTracker.Application.Wallets;

public static class ApplicationWalletErrors
{
    public static readonly Error WalletNotFound = new(
        "App.Wallet.NotFound",
        "Wallet with the specified id was not found."
    );
    
    public static readonly Error ConcurrencyConflict = new(
        "App.Wallet.ConcurrencyConflict",
        "Wallet was modified by another operation. Reload and retry."
    );
}