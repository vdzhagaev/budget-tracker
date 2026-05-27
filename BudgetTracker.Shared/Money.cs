namespace BudgetTracker.Shared;

public record Money
{
    public decimal Amount { get; init; }
    public Currency Currency { get; init; }
    
    private Money(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }
    
    public static Result<Money> Create(decimal amount, Currency currency)
    {
        if (amount < 0)
        {
            return Result<Money>.Failure(SharedErrors.Money.NegativeOrZero);
        }

        if (!Enum.IsDefined(currency))
        {
            return Result<Money>.Failure(SharedErrors.Money.InvalidCurrency);
        }

        return Result<Money>.Success(new Money(amount, currency));
    }
}