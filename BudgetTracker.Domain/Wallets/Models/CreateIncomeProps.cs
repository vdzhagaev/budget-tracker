using BudgetTracker.Domain.Wallets.Enums;
using BudgetTracker.Shared;

namespace BudgetTracker.Domain.Wallets.Models;

public record CreateIncomeProps(
    decimal Amount,
    Currency Currency,
    IncomeCategory Category,
    DateTime? Date = null
);