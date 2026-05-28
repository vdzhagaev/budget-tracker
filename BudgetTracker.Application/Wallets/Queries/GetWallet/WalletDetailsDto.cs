using BudgetTracker.Shared;

namespace BudgetTracker.Application.Wallets.Queries.GetWallet;

public sealed record WalletDetailsDto(Guid WalletId, string Name, Currency Currency, decimal Balance);