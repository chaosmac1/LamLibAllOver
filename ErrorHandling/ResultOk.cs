namespace LamLibAllOver.ErrorHandling;

public readonly struct ResultOk<T> : IEResult, IGetOk<T>, IResultSwitch<T, object> {
    private readonly EResult Status;
    private readonly bool StatusSet;
    private readonly T Value;

    private ResultOk(bool statusSet, T value, EResult status) {
        StatusSet = statusSet;
        Value = value;
        Status = status;
    }

    public T Ok() {
        if (Status == EResult.Ok && StatusSet)
            return Value;
        throw new Exception("Status Not Ok Or StatusSet Is False");
    }

    public T OkOr(T value) {
        if (StatusSet == false || Status == EResult.Err) return value;
        return Value;
    }

    public T? OkOrDefault() {
        if (StatusSet == false || Status == EResult.Err) return default;
        return Value;
    }

    public bool Equals(EResult status) {
        return Status == status;
    }

    public ResultOpen<T, object> Unwrap() {
        return new ResultOpen<T, object>(Status, Value, null);
    }

    public override bool Equals(object? obj) {
        return obj is EResult other && Equals(other);
    }

    public override int GetHashCode() {
        return HashCode.Combine((int)Status, StatusSet);
    }

    public ResultOk<TOk> AndThen<TOk>(Func<T, ResultOk<TOk>> func) {
        if (Status == EResult.Err)
            return ResultOk<TOk>.Err();
        return func(Value);
    }

    public ResultOk<TOk> And<TOk>(Func<ResultOk<TOk>> action) {
        if (Status == EResult.Err)
            return ResultOk<TOk>.Err();
        return action();
    }

    public ResultOk<TOk> Map<TOk>(Func<T, TOk> func) {
        if (Status == EResult.Err)
            return ResultOk<TOk>.Err();
        return ResultOk<TOk>.Ok(func(Ok()));
    }

    public async Task<ResultOk<TOk>> AndThenAsync<TOk>(Func<T, Task<ResultOk<TOk>>> func) {
        if (Status == EResult.Err)
            return ResultOk<TOk>.Err();
        return await func(Value);
    }

    public async Task<ResultOk<TOk>> AndAsync<TOk>(Func<Task<ResultOk<TOk>>> action) {
        if (Status == EResult.Err)
            return ResultOk<TOk>.Err();
        return await action();
    }

    public async Task<ResultOk<TOk>> MapAsync<TOk>(Func<T, Task<TOk>> func) {
        if (Status == EResult.Err)
            return ResultOk<TOk>.Err();
        return ResultOk<TOk>.Ok(await func(Ok()));
    }

    public static ResultOk<T> Empty() {
        return new();
    }

    public static ResultOk<T> Err() {
        return new(true, default!, EResult.Err);
    }

    public static ResultOk<T> Ok(T value) {
        return new(true, value, EResult.Ok);
    }

    public ResultNone ToNone() {
        return Status == EResult.Ok ? ResultNone.Ok : ResultNone.Err;
    }

    public static bool operator ==(ResultOk<T> result, EResult status) {
        return result.Status == status;
    }

    public static bool operator !=(ResultOk<T> result, EResult status) {
        return result.Status != status;
    }

    public TRes Match<TRes>(Func<TRes> thenErr, Func<T, TRes> thenOk) {
        return this == EResult.Err ? thenOk(Ok()) : thenErr();
    }
}