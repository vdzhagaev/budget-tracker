using BudgetTracker.Domain.Wallets.Errors;
using BudgetTracker.Shared;

namespace BudgetTracker.Domain.Wallets.ValueObjects;

public record Balance
{
   public decimal Amount { get; init; }
   public Currency Currency { get; init; }
   
   private Balance(decimal amount, Currency currency)
   {
      Amount = amount;
      Currency = currency;
   }

   public static Result<Balance> Create(decimal amount, Currency currency)
   {
      if (!Enum.IsDefined(currency))
      {
         return Result<Balance>.Failure(SharedErrors.Money.InvalidCurrency);
      }

      return Result<Balance>.Success(new Balance(amount, currency));
   }
   
   public static Balance Zero(Currency currency) => Create(0m, currency).Value;

   public Result<Balance> Add(Money money)
   {
      if (money.Currency != Currency)
      {
         return Result<Balance>.Failure(WalletErrors.CurrencyMismatch);
      }

      return Create(Amount + money.Amount, Currency);
   }
   
   public Result<Balance> Subtract(Money money)
   {
      if (money.Currency != Currency)
      {
         return Result<Balance>.Failure(WalletErrors.CurrencyMismatch);
      }
      
      return Create(Amount - money.Amount, Currency);
   }
};