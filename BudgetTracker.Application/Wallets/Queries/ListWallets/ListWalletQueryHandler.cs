namespace BudgetTracker.Application.Wallets.Queries.ListWallets;

public class ListWalletQueryHandler
{
    private readonly IWalletRepository _repository;
    public ListWalletQueryHandler(IWalletRepository repository) =>
        _repository = repository;

    public async Task<IReadOnlyList<WalletSummaryDto>> HandleAsync(ListWalletsQuery query, CancellationToken ct)
    {
        var wallets = await _repository.ListAllAsync(ct);
        return wallets.Count == 0 ? [] : wallets
            .OrderBy(w => w.Name, StringComparer.OrdinalIgnoreCase)
            .Select(w => w.ToWalletSummaryDto())
            .ToList();
    }
}