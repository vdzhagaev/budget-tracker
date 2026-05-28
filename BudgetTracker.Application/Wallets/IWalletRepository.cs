using BudgetTracker.Domain.Wallets;

namespace BudgetTracker.Application.Wallets;

public interface IWalletRepository
{ 
    Task<IReadOnlyList<Wallet>> ListAllAsync(CancellationToken ct);
    Task<Wallet?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(Wallet wallet, CancellationToken ct);
    Task UpdateAsync(Wallet wallet, CancellationToken ct);
}