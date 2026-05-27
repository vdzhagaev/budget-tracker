using BudgetTracker.Domain.Wallets.Enums;
using BudgetTracker.Shared;

namespace BudgetTracker.Application.Wallets.Commands.AddIncome;

public sealed record AddIncomeCommand(
    Guid WalletId, 
    decimal Amount, 
    Currency Currency, 
    IncomeCategory Category, 
    DateTime? Date
);