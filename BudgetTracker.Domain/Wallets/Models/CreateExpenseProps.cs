using BudgetTracker.Domain.Wallets.Enums;
using BudgetTracker.Shared;

namespace BudgetTracker.Domain.Wallets.Models;

public record CreateExpenseProps(
    decimal Amount,
    Currency Currency,
    ExpenseCategory Category,
    DateTime? Date = null
);