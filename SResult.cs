namespace LamLibAllOver;

public readonly struct SResult<OK> : IEResult, IGetOk<OK>, IGetErr<SErrHolder>, IResultSwitch<OK, SErrHolder> {
    private readonly EResult Status;
    private readonly bool StatusSet;
    private readonly OK Value;
    private readonly SErrHolder Value2;

    public SResult(EResult status, bool statusSet, OK value, SErrHolder value2) {
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

    public SErrHolder Err() {
        return Status == EResult.Err && StatusSet
            ? Value2
            : throw new Exception("Status Not Err Or StatusSet Is False");
    }

    public bool Equals(EResult status) {
        return Status == status;
    }

    public ResultOpen<OK, SErrHolder> Unwrap() {
        return new ResultOpen<OK, SErrHolder>(Status, Value, Value2);
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

    public SErrHolder ErrOr(SErrHolder value) {
        if (StatusSet || Status == EResult.Ok) return value;
        return Value2;
    }

    public OK? OkOrDefault() {
        if (StatusSet == false || Status == EResult.Err) return default;
        return Value;
    }

    public SErrHolder? ErrOrDefault() {
        if (StatusSet || Status == EResult.Ok) return default;
        return Value2;
    }

    public SResult<TOK2> AndThen<TOK2>(Func<OK, SResult<TOK2>> func) {
        if (Status == EResult.Err)
            return SResult<TOK2>.Err(Err());
        return func(Value);
    }

    public SResult<TOK2> And<TOK2>(Func<SResult<TOK2>> action) {
        if (Status == EResult.Err)
            return SResult<TOK2>.Err(Err());
        return action();
    }

    public SResult<TOK2> Map<TOK2>(Func<OK, TOK2> func) {
        return Status == EResult.Err
            ? SResult<TOK2>.Err(Err())
            : SResult<TOK2>.Ok(func(Ok()));
    }

    public SResult<OK> MapErr(Func<SErrHolder, SErrHolder> func) {
        if (Status == EResult.Ok)
            return Ok(Ok());
        return Err(func(Err()));
    }

    public async ValueTask<SResult<TOK2>> AndThenAsync<TOK2>(Func<OK, ValueTask<SResult<TOK2>>> func) {
        if (Status == EResult.Err)
            return SResult<TOK2>.Err(Err());
        return await func(Value);
    }

    public async ValueTask<SResult<TOK2>> AndAsync<TOK2>(Func<ValueTask<SResult<TOK2>>> action) {
        if (Status == EResult.Err)
            return SResult<TOK2>.Err(Err());
        return await action();
    }

    public async Task<SResult<TOK2>> MapAsync<TOK2>(Func<OK, Task<TOK2>> func) {
        return Status == EResult.Err
            ? SResult<TOK2>.Err(Err())
            : SResult<TOK2>.Ok(await func(Ok()));
    }

    public async ValueTask<SResult<OK>> MapErrAsync(Func<SErrHolder, ValueTask<SErrHolder>> func) {
        if (Status == EResult.Ok)
            return Ok(Ok());

        return Err(await func(Err()));
    }


    public static SResult<OK> Empty => new();

    public static SResult<OK> Err(SErrHolder value) {
        return new SResult<OK>(EResult.Err, true, default!, value);
    }

    public static SResult<OK> Ok(OK value) {
        return new SResult<OK>(EResult.Ok, true, value, default!);
    }

    public ResultNone ToNone() {
        return Status == EResult.Ok ? ResultNone.Ok : ResultNone.Err;
    }

    public static implicit operator ResultOk<OK>(SResult<OK> result) {
        return result == EResult.Err
            ? ResultOk<OK>.Err()
            : ResultOk<OK>.Ok(result.Ok());
    }

    public static implicit operator ResultErr<SErrHolder>(SResult<OK> result) {
        return result == EResult.Err
            ? ResultErr<SErrHolder>.Err(result.Err())
            : ResultErr<SErrHolder>.Ok();
    }

    public static bool operator ==(SResult<OK> result, EResult status) {
        return result.Status == status;
    }

    public static bool operator !=(SResult<OK> result, EResult status) {
        return result.Status != status;
    }

    public SResult<OK2> ChangeOkType<OK2>() {
        return SResult<OK2>.Err(Err());
    }


    public EResult Out(out SErrHolder? err) {
        if (this == EResult.Ok) {
            err = default;
            return EResult.Ok;
        }

        err = Value2;
        return EResult.Err;
    }
}