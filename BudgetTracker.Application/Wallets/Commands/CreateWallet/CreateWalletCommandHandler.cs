using BudgetTracker.Domain.Wallets;
using BudgetTracker.Shared;

namespace BudgetTracker.Application.Wallets.Commands.CreateWallet;

public sealed class CreateWalletCommandHandler
{
    private readonly IWalletRepository _repository;

    public CreateWalletCommandHandler(IWalletRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Guid>> HandleAsync(CreateWalletCommand command, CancellationToken ct)
    {
        var walletResult = Wallet.Create(command.Name, command.Currency);
        if (!walletResult.IsSuccess) return Result<Guid>.Failure(walletResult.Error);
        await _repository.AddAsync(walletResult.Value, ct);
        return Result<Guid>.Success(walletResult.Value.Id);
    }
}