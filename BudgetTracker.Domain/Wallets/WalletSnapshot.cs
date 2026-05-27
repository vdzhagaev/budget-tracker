using BudgetTracker.Domain.Wallets.Entities;
using BudgetTracker.Domain.Wallets.ValueObjects;
using BudgetTracker.Shared;

namespace BudgetTracker.Domain.Wallets;

public sealed record WalletSnapshot(
    Guid Id,
    string Name,
    Currency Currency,
    Balance Balance,
    uint Version,
    IEnumerable<Expense> Expenses,
    IEnumerable<Income> Incomes);