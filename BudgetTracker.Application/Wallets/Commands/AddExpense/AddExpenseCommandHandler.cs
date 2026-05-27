using BudgetTracker.Application.Wallets.Exceptions;
using BudgetTracker.Domain.Wallets.Models;
using BudgetTracker.Shared;

namespace BudgetTracker.Application.Wallets.Commands.AddExpense;

public sealed class AddExpenseCommandHandler
{
    private readonly IWalletRepository _repository;
    public AddExpenseCommandHandler(IWalletRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Guid>> HandleAsync(AddExpenseCommand command, CancellationToken ct)
    {
        var wallet = await _repository.GetByIdAsync(command.WalletId, ct);
        if (wallet is null) return Result<Guid>.Failure(ApplicationWalletErrors.WalletNotFound);
        

        var expenseProps = new CreateExpenseProps(
            Amount: command.Amount,
            Currency: command.Currency,
            Category: command.Category,
            Date: command.Date
        );
        
        var mutateResult = wallet.AddExpense(expenseProps);
        if (!mutateResult.IsSuccess) return Result<Guid>.Failure(mutateResult.Error);
        try
        {
            await _repository.UpdateAsync(wallet, ct);
        }
        catch (ConcurrencyConflictException)
        {
            return Result<Guid>.Failure(ApplicationWalletErrors.ConcurrencyConflict);
        }
        return Result<Guid>.Success(mutateResult.Value);
    }
}