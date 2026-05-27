using BudgetTracker.Domain.Wallets.Enums;
using BudgetTracker.Domain.Wallets.Errors;
using BudgetTracker.Domain.Wallets.Models;
using BudgetTracker.Shared;

namespace BudgetTracker.Domain.Wallets.Entities;

public class Income : Transaction
{
    public IncomeCategory IncomeCategory { get; }
    
    private Income(Guid id, Money amount, IncomeCategory incomeCategory, DateTime date) 
        : base(id, amount, date) 
    {
        IncomeCategory = incomeCategory;
    }

    public static Result<Income> Create(CreateIncomeProps props)
    {
        var moneyResult = Money.Create(props.Amount, props.Currency);
        if (!moneyResult.IsSuccess)
        {
            return Result<Income>.Failure(moneyResult.Error);
        }
        var money = moneyResult.Value;

        if (!Enum.IsDefined(props.Category))
        {
            return Result<Income>.Failure(TransactionErrors.CategoryNotFound);
        }
        
        var incomeDate = props.Date ?? DateTime.Today;
        if (props.Date > DateTime.Today)
        {
            return Result<Income>.Failure(TransactionErrors.FutureDate);
        }
        
        var income = new Income(Guid.NewGuid(), money, props.Category, incomeDate);
        return Result<Income>.Success(income);
    }
}