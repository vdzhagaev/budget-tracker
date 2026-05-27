using BudgetTracker.Shared;

namespace BudgetTracker.Application.Wallets.Commands.CreateWallet;

public sealed record CreateWalletCommand(string Name, Currency Currency);