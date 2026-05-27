using AwesomeAssertions;
using BudgetTracker.Domain.Wallets.Errors;
using BudgetTracker.Domain.Wallets.ValueObjects;
using BudgetTracker.Shared;

namespace BudgetTracker.Domain.Tests.Wallets.ValueObjects;

public sealed class BalanceTests
{
    [Fact]
    public void Zero_should_return_balance_with_zero_amount_for_given_currency()
    {
        var balance = Balance.Zero(Currency.USD);

        balance.Amount.Should().Be(0m);
        balance.Currency.Should().Be(Currency.USD);
    }

    [Fact]
    public void Create_should_return_success_for_valid_currency()
    {
        var balance = Balance.Create(100m, Currency.USD);

        balance.IsSuccess.Should().BeTrue();

        var result = balance.Value;
        result.Amount.Should().Be(100m);
        result.Currency.Should().Be(Currency.USD);
    }

    [Fact]
    public void Create_should_fail_for_invalid_currency()
    {
        var balance = Balance.Create(100m, (Currency)999);

        balance.IsSuccess.Should().BeFalse();
        balance.Error.Should().Be(SharedErrors.Money.InvalidCurrency);
    }

    [Fact]
    public void Create_should_allow_negative_amount()
    {
        var balance = Balance.Create(-100m, Currency.USD);

        balance.IsSuccess.Should().BeTrue();

        var result = balance.Value;
        result.Amount.Should().Be(-100m);
    }

    [Fact]
    public void Add_should_return_new_balance_with_summed_amount_for_same_currency()
    {
        var balance = Balance.Create(100m, Currency.USD).Value;

        var money = Money.Create(50m, Currency.USD).Value;

        var result = balance.Add(money);
        result.IsSuccess.Should().BeTrue();

        result.Value.Amount.Should().Be(150m);
        result.Value.Currency.Should().Be(Currency.USD);
        balance.Amount.Should().Be(100m);
    }

    [Fact]
    public void Add_should_fail_when_currency_does_not_match()
    {
        var balance = Balance.Create(100m, Currency.USD).Value;

        var money = Money.Create(100m, Currency.EUR).Value;

        var result = balance.Add(money);
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(WalletErrors.CurrencyMismatch);
    }

    [Fact]
    public void Subtract_should_return_new_balance_with_difference_for_same_currency()
    {
        var balance = Balance.Create(100m, Currency.USD).Value;
        var money = Money.Create(50m, Currency.USD).Value;

        var result = balance.Subtract(money);
        result.IsSuccess.Should().BeTrue();

        result.Value.Amount.Should().Be(50m);
        result.Value.Currency.Should().Be(Currency.USD);
        balance.Amount.Should().Be(100m);
    }

    [Fact]
    public void Subtract_should_fail_when_currency_does_not_match()
    {
        var balance = Balance.Create(100m, Currency.USD).Value;

        var money = Money.Create(100m, Currency.EUR).Value;

        var result = balance.Subtract(money);
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(WalletErrors.CurrencyMismatch);
    }

    [Fact]
    public void Subtract_can_produce_negative_balance()
    {
        var balance = Balance.Create(50m, Currency.USD).Value;
        var money = Money.Create(80m, Currency.USD).Value;

        var result = balance.Subtract(money);

        result.IsSuccess.Should().BeTrue();
        result.Value.Amount.Should().Be(-30m);
    }
}