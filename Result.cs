namespace LamLibAllOver;

public readonly struct Result<OK, ERR> : IEResult, IGetOk<OK>, IGetErr<ERR>, IResultSwitch<OK, ERR> {
    private readonly EResult Status;
    private readonly bool StatusSet;
    private readonly OK Value;
    private readonly ERR Value2;

    public Result(EResult status, bool statusSet, OK value, ERR value2) {
        Status = status;
        StatusSet = statusSet;
        Value = value;
        Value2 = value2;
    }

    public OK Ok() {
        return Status == EResult.Ok && StatusSet
            ? Value
            : throw new Exception("Status Not Ok Or StatusSet Is False");
    }

    public ERR Err() {
        return Status == EResult.Err && StatusSet
            ? Value2
            : throw new Exception("Status Not Err Or StatusSet Is False");
    }

    public bool Equals(EResult status) {
        return Status == status;
    }

    public ResultOpen<OK, ERR> Unwrap() {
        return new ResultOpen<OK, ERR>(Status, Value, Value2);
    }

    public override bool Equals(object? obj) {
        return obj is EResult other && Equals(other);
    }

    public override int GetHashCode() {
        return HashCode.Combine((int)Status, StatusSet);
    }

    public OK OkOr(OK value) {
        if (StatusSet == false || Status == EResult.Err) return value;
        return Value;
    }

    public ERR ErrOr(ERR value) {
        if (StatusSet || Status == EResult.Ok) return value;
        return Value2;
    }

    public OK? OkOrDefault() {
        if (StatusSet == false || Status == EResult.Err) return default;
        return Value;
    }

    public ERR? ErrOrDefault() {
        if (StatusSet || Status == EResult.Ok) return default;
        return Value2;
    }

    public Result<TOK2, ERR> AndThen<TOK2>(Func<OK, Result<TOK2, ERR>> func) {
        if (Status == EResult.Err)
            return Result<TOK2, ERR>.Err(Err());
        return func(Value);
    }

    public Result<TOK2, ERR> And<TOK2>(Func<Result<TOK2, ERR>> action) {
        if (Status == EResult.Err)
            return Result<TOK2, ERR>.Err(Err());
        return action();
    }

    public Result<TOK2, ERR> Map<TOK2>(Func<OK, TOK2> func) {
        return Status == EResult.Err
            ? Result<TOK2, ERR>.Err(Err())
            : Result<TOK2, ERR>.Ok(func(Ok()));
    }

    public Result<OK, TERR2> MapErr<TERR2>(Func<ERR, TERR2> func) {
        if (Status == EResult.Ok)
            return Result<OK, TERR2>.Ok(Ok());
        return Result<OK, TERR2>.Err(func(Err()));
    }

    public async Task<Result<TOK2, ERR>> AndThenAsync<TOK2>(Func<OK, Task<Result<TOK2, ERR>>> func) {
        if (Status == EResult.Err)
            return Result<TOK2, ERR>.Err(Err());
        return await func(Value);
    }

    public async Task<Result<TOK2, ERR>> AndAsync<TOK2>(Func<Task<Result<TOK2, ERR>>> action) {
        if (Status == EResult.Err)
            return Result<TOK2, ERR>.Err(Err());
        return await action();
    }

    public async Task<Result<TOK2, ERR>> MapAsync<TOK2>(Func<OK, Task<TOK2>> func) {
        return Status == EResult.Err
            ? Result<TOK2, ERR>.Err(Err())
            : Result<TOK2, ERR>.Ok(await func(Ok()));
    }

    public async Task<Result<OK, TERR2>> MapErrAsync<TERR2>(Func<ERR, Task<TERR2>> func) {
        if (Status == EResult.Ok)
            return Result<OK, TERR2>.Ok(Ok());
        return Result<OK, TERR2>.Err(await func(Err()));
    }

    public static Result<OK, ERR> Empty => new();

    public static Result<OK, ERR> Err(ERR value) {
        return new(EResult.Err, true, default!, value);
    }

    public static Result<OK, ERR> Ok(OK value) {
        return new(EResult.Ok, true, value, default!);
    }

    public ResultNone ToNone() {
        return Status == EResult.Ok ? ResultNone.Ok : ResultNone.Err;
    }

    public static implicit operator ResultOk<OK>(Result<OK, ERR> result) {
        return result == EResult.Err
            ? ResultOk<OK>.Err()
            : ResultOk<OK>.Ok(result.Ok());
    }

    public static implicit operator ResultErr<ERR>(Result<OK, ERR> result) {
        return result == EResult.Err
            ? ResultErr<ERR>.Err(result.Err())
            : ResultErr<ERR>.Ok();
    }

    public static bool operator ==(Result<OK, ERR> result, EResult status) {
        return result.Status == status;
    }

    public static bool operator !=(Result<OK, ERR> result, EResult status) {
        return result.Status != status;
    }
}