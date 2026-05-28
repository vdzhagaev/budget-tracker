using BudgetTracker.Shared;

namespace BudgetTracker.Application.Wallets.Queries.GetWallet;

public sealed record WalletDetailsDto(
    Guid Id, 
    string Name, 
    string Currency, 
    decimal Balance,
    uint Version,
    decimal IncomesTotal,
    decimal ExpensesTotal,
    int TransactionsTotal
    );