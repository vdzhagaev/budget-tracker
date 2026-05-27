using BudgetTracker.Domain.Wallets.Entities;
using BudgetTracker.Domain.Wallets.Errors;
using BudgetTracker.Domain.Wallets.Models;
using BudgetTracker.Domain.Wallets.ValueObjects;
using BudgetTracker.Shared;

namespace BudgetTracker.Domain.Wallets;

public class Wallet
{
    private readonly List<Expense> _expenses = [];

    private readonly List<Income> _incomes = [];

    private Wallet(Guid id, string name, Currency currency, Balance balance, uint version)
    {
        Id = id;
        Name = name;
        Currency = currency;
        Balance = balance;
        Version = version;
    }

    public Guid Id { get; private init; }
    public string Name { get; private set; }
    public Currency Currency { get; }
    public uint Version { get; private set; }

    public IReadOnlyCollection<Income> Incomes => _incomes.AsReadOnly();
    public IReadOnlyCollection<Expense> Expenses => _expenses.AsReadOnly();

    public Balance Balance { get; private set; }

    public static Result<Wallet> Create(string name, Currency currency)
    {
        if (string.IsNullOrWhiteSpace(name)) return Result<Wallet>.Failure(WalletErrors.EmptyName);
        if (!Enum.IsDefined(currency)) return Result<Wallet>.Failure(SharedErrors.Money.InvalidCurrency);

        var wallet = new Wallet(
            Guid.NewGuid(),
            name, currency,
            Balance.Zero(currency),
            0);
        return Result<Wallet>.Success(wallet);
    }

    public static Wallet Reconstruct(WalletSnapshot snapshot)
    {
        var wallet = new Wallet(snapshot.Id, snapshot.Name, snapshot.Currency, snapshot.Balance, snapshot.Version);

        wallet._expenses.AddRange(snapshot.Expenses);
        wallet._incomes.AddRange(snapshot.Incomes);
        return wallet;
    }

    public Result<Guid> AddIncome(CreateIncomeProps props)
    {
        if (props.Currency != Currency) return Result<Guid>.Failure(WalletErrors.CurrencyMismatch);

        var incomeResult = Income.Create(props);
        if (!incomeResult.IsSuccess) return Result<Guid>.Failure(incomeResult.Error);

        var newBalance = Balance.Add(incomeResult.Value.Money);
        if (!newBalance.IsSuccess) return Result<Guid>.Failure(newBalance.Error);

        _incomes.Add(incomeResult.Value);
        Balance = newBalance.Value;

        Version++;
        return Result<Guid>.Success(incomeResult.Value.Id);
    }

    public Result<Guid> AddExpense(CreateExpenseProps props)
    {
        if (props.Currency != Currency) return Result<Guid>.Failure(WalletErrors.CurrencyMismatch);

        var expenseResult = Expense.Create(props);
        if (!expenseResult.IsSuccess) return Result<Guid>.Failure(expenseResult.Error);

        if (Balance.Amount < expenseResult.Value.Money.Amount) return Result<Guid>.Failure(WalletErrors.InsufficientFunds);

        var newBalance = Balance.Subtract(expenseResult.Value.Money);
        if (!newBalance.IsSuccess) return Result<Guid>.Failure(newBalance.Error);

        _expenses.Add(expenseResult.Value);
        Balance = newBalance.Value;

        Version++;
        return Result<Guid>.Success(expenseResult.Value.Id);
    }
}