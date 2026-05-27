using AwesomeAssertions;
using BudgetTracker.Domain.Wallets;
using BudgetTracker.Domain.Wallets.Entities;
using BudgetTracker.Domain.Wallets.Enums;
using BudgetTracker.Domain.Wallets.Errors;
using BudgetTracker.Domain.Wallets.Models;
using BudgetTracker.Domain.Wallets.ValueObjects;
using BudgetTracker.Shared;

namespace BudgetTracker.Domain.Tests.Wallets;

public sealed class WalletTests
{
    private static Wallet ValidWallet(Currency currency = Currency.USD)
    {
        return Wallet.Create("Test Wallet", currency).Value;
    }

    private static CreateIncomeProps IncomeProps(
        decimal amount,
        Currency currency = Currency.USD,
        IncomeCategory category = IncomeCategory.Other)
    {
        return new CreateIncomeProps(amount, currency, category);
    }

    private static CreateExpenseProps ExpenseProps(
        decimal amount,
        Currency currency = Currency.USD,
        ExpenseCategory category = ExpenseCategory.Other)
    {
        return new CreateExpenseProps(amount, currency, category);
    }

    [Fact]
    public void Create_succeeds_with_valid_name_and_currency()
    {
        var result = Wallet.Create("My Wallet", Currency.USD);
        result.IsSuccess.Should().BeTrue();
        var wallet = result.Value;
        wallet.Name.Should().Be("My Wallet");
        wallet.Currency.Should().Be(Currency.USD);
        wallet.Balance.Amount.Should().Be(0m);
        wallet.Balance.Currency.Should().Be(Currency.USD);
        wallet.Version.Should().Be(0u);
        wallet.Incomes.Should().BeEmpty();
        wallet.Expenses.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData("\t\n")]
    public void Create_fails_when_name_is_empty_or_whitespace(string name)
    {
        var result = Wallet.Create(name, Currency.USD);
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(WalletErrors.EmptyName);
    }

    [Fact]
    public void Create_fails_with_invalid_currency()
    {
        var result = Wallet.Create("My Wallet", (Currency)99);
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(SharedErrors.Money.InvalidCurrency);
    }

    [Fact]
    public void AddIncome_increases_balance_and_version_on_success()
    {
        var wallet = ValidWallet();
        var result = wallet.AddIncome(IncomeProps(100m));
        result.IsSuccess.Should().BeTrue();
        wallet.Version.Should().Be(1u);
        wallet.Incomes.Should().HaveCount(1);
        wallet.Balance.Amount.Should().Be(100m);
    }
    
    [Fact]
    public void AddExpense_decreases_balance_and_version_on_success()
    {
        var wallet = ValidWallet();
        wallet.AddIncome(IncomeProps(100m));
        var result = wallet.AddExpense(ExpenseProps(39m));
        result.IsSuccess.Should().BeTrue();
        wallet.Version.Should().Be(2u);
        wallet.Expenses.Should().HaveCount(1);
        wallet.Balance.Amount.Should().Be(61m);
    }

    [Fact]
    public void AddExpense_fails_with_insufficient_funds_and_does_not_mutate_state()
    {
        var wallet = ValidWallet();
        var result = wallet.AddExpense(ExpenseProps(50m));
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(WalletErrors.InsufficientFunds);
        wallet.Version.Should().Be(0u);
        wallet.Expenses.Should().BeEmpty();
    }

    [Fact]
    public void AddIncome_fails_with_currency_mismatch_and_does_not_mutate_state()
    {
        var wallet = ValidWallet();
        var result = wallet.AddIncome(IncomeProps(100m, Currency.EUR));
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(WalletErrors.CurrencyMismatch);
        wallet.Version.Should().Be(0u);
        wallet.Incomes.Should().BeEmpty();
    }

    [Fact]
    public void AddExpense_fails_with_currency_mismatch_and_does_not_mutate_state()
    {
        var wallet = ValidWallet();
        wallet.AddIncome(IncomeProps(100m));
        var result = wallet.AddExpense(ExpenseProps(100m, Currency.EUR));
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(WalletErrors.CurrencyMismatch);
        wallet.Version.Should().Be(1u);
        wallet.Expenses.Should().BeEmpty();
    }

    [Fact]
    public void AddExpense_succeeds_when_balance_equals_expense_amount_reaching_zero()
    {
        var wallet = ValidWallet();
        wallet.AddIncome(IncomeProps(100m));
        var result = wallet.AddExpense(ExpenseProps(100m));
        result.IsSuccess.Should().BeTrue();
        wallet.Version.Should().Be(2u);
        wallet.Balance.Amount.Should().Be(0m);
    }

    [Fact]
    public void AddIncome_then_AddExpense_produces_expected_balance_and_version()
    {
        var wallet = ValidWallet();
        wallet.AddIncome(IncomeProps(100m));
        wallet.AddIncome(IncomeProps(100m));
        wallet.AddExpense(ExpenseProps(80m));
        wallet.Balance.Amount.Should().Be(120m);
        wallet.Version.Should().Be(3u);
        wallet.Incomes.Should().HaveCount(2);
        wallet.Expenses.Should().HaveCount(1);
    }

    [Fact]
    public void Reconstruct_restores_full_state_from_snapshot()
    {
        var id = Guid.NewGuid();
        var income = Income.Create(IncomeProps(100m)).Value;
        var expense = Expense.Create(ExpenseProps(100m)).Value;
        var snapshot = new WalletSnapshot(
            Id: id,
            Name: "Wallet",
            Balance: Balance.Create(100m, Currency.USD).Value,
            Currency: Currency.USD,
            Version: 3u,
            Incomes: [income],
            Expenses: [expense]
        );
        
        var wallet = Wallet.Reconstruct(snapshot);
        wallet.Id.Should().Be(id);
        wallet.Name.Should().Be("Wallet");
        wallet.Currency.Should().Be(Currency.USD);
        wallet.Balance.Amount.Should().Be(100m);
        wallet.Balance.Currency.Should().Be(Currency.USD);
        wallet.Version.Should().Be(3u);
        wallet.Incomes.Should().ContainSingle().Which.Should().BeSameAs(income);
        wallet.Expenses.Should().ContainSingle().Which.Should().BeSameAs(expense);
    }
}