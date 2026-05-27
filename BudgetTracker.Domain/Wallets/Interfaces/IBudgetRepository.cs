using BudgetTracker.Domain.Wallets.Entities;
using BudgetTracker.Shared;

namespace BudgetTracker.Domain.Interfaces;

public interface IBudgetRepository
{
    Task<Result<Expense>> GetById(Guid id);
    Task<Result<IEnumerable<Expense>>> GetAll();
    Task<Result> Add(Expense expense);
    Task<Result> Update(Expense expense);
    Task<Result> Delete(Expense expense);
}