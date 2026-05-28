namespace BudgetTracker.Application.Wallets.Queries.GetWallet;

public sealed class GetWalletQueryHandler
{
    private readonly IWalletRepository _repository;
    
    public GetWalletQueryHandler(IWalletRepository repository) =>
        _repository = repository;
    
    public async Task<WalletDetailsDto?> HandleAsync(GetWalletQuery query, CancellationToken ct)
    {
        var wallet = await _repository.GetByIdAsync(query.WalletId, ct);
        return wallet?.ToDetailsDto();
    }
}