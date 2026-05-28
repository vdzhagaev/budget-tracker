namespace BudgetTracker.Application.Wallets.Queries.ListWallets;

public record WalletSummaryDto(Guid Id, string Name, string Currency, decimal Balance);