namespace BudgetTracker.Application.Wallets.Queries.ListWallets;

public sealed record WalletSummaryDto(Guid Id, string Name, string Currency, decimal Balance);