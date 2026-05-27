using BudgetTracker.Domain.Wallets.Enums;
using BudgetTracker.Domain.Wallets.Errors;
using BudgetTracker.Shared;

namespace BudgetTracker.Domain.Wallets.Entities;

public abstract class Transaction(Guid id, Money amount, DateTime date)
{
    public Guid Id { get; protected set; } = id;
    public Money Money { get; protected set; } = amount;
    public DateTime Date { get;protected set; } = date;
}