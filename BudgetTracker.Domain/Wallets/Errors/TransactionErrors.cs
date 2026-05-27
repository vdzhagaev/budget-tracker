using BudgetTracker.Shared;

namespace BudgetTracker.Domain.Wallets.Errors;

public static class TransactionErrors
{
    public static readonly Error FutureDate = new(
        "Transaction.FutureDate",
        "Date of transaction cannot be in the future."
    );

    public static readonly Error CategoryNotFound = new("Category.NotFound", "Category does not exist");
}