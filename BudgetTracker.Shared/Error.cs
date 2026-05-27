namespace BudgetTracker.Shared;

public record Error(string Code, string Message)
{
    public static readonly Error Empty = new(string.Empty, "Empty Error");
    public static readonly Error Unknown = new Error("Unknown", "Unknown Error");
    
    public override string ToString() => $"{Code}: {Message}";
}

public static class SharedErrors
{
    public static class Money
    {
        public static readonly Error NegativeOrZero = new Error("Money.Negative", "The amount cannot be negative or zero");
        public static readonly Error InvalidCurrency = new Error("Money.InvalidCurrency", "Unsupported currency");
    }
}