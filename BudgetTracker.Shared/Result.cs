namespace BudgetTracker.Shared;

public class Result
{
    public bool IsSuccess { get; }
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.Empty)
        {
            throw new InvalidOperationException("Result is successful and with error");
        }
        IsSuccess = isSuccess;
        Error = error;
    }
    
    public static Result Success() => new(true, Error.Empty);
    public static Result Failure(Error error) => new(false, error);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    private Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _value = value;
    }
    
    public TValue Value => IsSuccess
    ? _value!
    : throw new InvalidOperationException("The value cannot be accessed on error.");

    public static Result<TValue> Success(TValue? value) => new(value, true, Error.Empty);
    public new static Result<TValue> Failure(Error error) => new(default, false, error);
}