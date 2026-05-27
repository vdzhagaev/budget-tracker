using BudgetTracker.Domain.Wallets.Enums;
using BudgetTracker.Domain.Wallets.Errors;
using BudgetTracker.Domain.Wallets.Models;
using BudgetTracker.Shared;

namespace BudgetTracker.Domain.Wallets.Entities;

public class Expense : Transaction
{
    public ExpenseCategory ExpenseCategory { get; }
    
    private Expense(Guid id, Money amount, ExpenseCategory expenseCategory, DateTime date) 
        : base(id, amount, date)
    {
        ExpenseCategory = expenseCategory;
    }

    public static Result<Expense> Create(CreateExpenseProps props)
    {
        var moneyResult = Money.Create(props.Amount, props.Currency);
        if (!moneyResult.IsSuccess)
        {
            return Result<Expense>.Failure(moneyResult.Error);
        }
        var money = moneyResult.Value;

        if (!Enum.IsDefined(props.Category))
        {
            return Result<Expense>.Failure(TransactionErrors.CategoryNotFound);
        }
        
        var expenseDate = props.Date ?? DateTime.Today;
        if (props.Date > DateTime.Today)
        {
            return Result<Expense>.Failure(TransactionErrors.FutureDate);
        }
        var expense = new Expense(Guid.NewGuid(), money, props.Category, expenseDate);
        return Result<Expense>.Success(expense);
    }
}