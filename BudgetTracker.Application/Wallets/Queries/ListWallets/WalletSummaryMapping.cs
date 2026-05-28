using BudgetTracker.Domain.Wallets;

namespace BudgetTracker.Application.Wallets.Queries.ListWallets;

public static class WalletSummaryMapping
{
    public static WalletSummaryDto ToWalletSummaryDto(this Wallet wallet)
    {
        return new WalletSummaryDto(
            Id: wallet.Id,
            Name: wallet.Name,
            Currency: wallet.Currency.ToString(),
            Balance: wallet.Balance.Amount
        );
    }
}