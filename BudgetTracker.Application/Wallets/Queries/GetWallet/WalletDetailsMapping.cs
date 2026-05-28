using BudgetTracker.Domain.Wallets;

namespace BudgetTracker.Application.Wallets.Queries.GetWallet;

public static class WalletDetailsMapping
{
    public static WalletDetailsDto ToDetailsDto(this Wallet wallet)
    {
        return new WalletDetailsDto(
            Id: wallet.Id,
            Name: wallet.Name,
            Currency: wallet.Currency.ToString(),
            Balance: wallet.Balance.Amount,
            Version: wallet.Version,
            IncomesTotal: wallet.Incomes.Sum(i => i.Money.Amount),
            ExpensesTotal: wallet.Expenses.Sum(e => e.Money.Amount),
            TransactionsCount: wallet.Incomes.Count + wallet.Expenses.Count
        );
    }
}