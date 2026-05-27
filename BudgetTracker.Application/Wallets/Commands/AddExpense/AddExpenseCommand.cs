using BudgetTracker.Domain.Wallets.Enums;
using BudgetTracker.Shared;

namespace BudgetTracker.Application.Wallets.Commands.AddExpense;

public sealed record AddExpenseCommand(
    Guid WalletId, 
    decimal Amount, 
    Currency Currency, 
    ExpenseCategory Category, 
    DateTime? Date
);