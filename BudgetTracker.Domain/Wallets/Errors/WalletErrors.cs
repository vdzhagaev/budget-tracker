using BudgetTracker.Shared;

namespace BudgetTracker.Domain.Wallets.Errors;

public static class WalletErrors
{
    public static readonly Error EmptyName = new(
        "Wallet.EmptyName",
        "Name of the wallet cannot be empty."
    );

    public static readonly Error CurrencyMismatch = new(
        "Wallet.CurrencyMismatch",
        "The currency does not match the currency in the wallet."
    );

    public static readonly Error InsufficientFunds = new(
        "Wallet.InsufficientFunds", 
        "There are not enough funds to add an expense to this wallet."
    );
}