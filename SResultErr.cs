namespace LamLibAllOver;

public readonly struct SResultErr : IEResult, IGetErr<SErrHolder>, IResultSwitch<object, SErrHolder> {
    private readonly EResult Status;
    private readonly bool StatusSet;
    private readonly SErrHolder Value2;

    private SResultErr(EResult status, bool statusSet, SErrHolder value2) {
        Status = status;
        StatusSet = statusSet;
        Value2 = value2;
    }

    public SErrHolder Err() {
        return Status == EResult.Err && StatusSet
            ? Value2
            : throw new Exception("Status Not Err Or StatusSet Is False");
    }

    public bool Equals(EResult status) {
        return Status == status;
    }

    public ResultOpen<object, SErrHolder> Unwrap() {
        return new ResultOpen<object, SErrHolder>(Status, null, Value2);
    }

    public override bool Equals(object? obj) {
        return obj is EResult other && Equals(other);
    }

    public override int GetHashCode() {
        return HashCode.Combine((int)Status, StatusSet);
    }

    public SErrHolder ErrOr(SErrHolder value) {
        if (StatusSet || Status == EResult.Ok) return value;
        return Value2;
    }

    public SErrHolder? ErrOrDefault() {
        if (StatusSet || Status == EResult.Ok) return default;
        return Value2;
    }

    public SResultErr AndThen(Func<SResultErr> func) {
        if (Status == EResult.Err)
            return this;
        return func();
    }

    public SResultErr And(Func<SResultErr> action) {
        if (Status == EResult.Err)
            return this;
        return action();
    }

    public SResultErr MapErr(Func<SErrHolder, SErrHolder> func) {
        if (Status == EResult.Ok)
            return this;
        return Err(func(Err()));
    }

    public async ValueTask<SResultErr> AndThenAsync(Func<ValueTask<SResultErr>> func) {
        if (Status == EResult.Err)
            return this;
        return await func();
    }

    public async ValueTask<SResult<OK>> AndThenAsync<OK>(Func<ValueTask<SResult<OK>>> func) {
        if (Status == EResult.Err)
            return ConvertTo<OK>();
        return await func();
    }

    public async ValueTask<SResultErr> MapErrAsync(Func<SErrHolder, ValueTask<SErrHolder>> func) {
        if (Status == EResult.Ok)
            return this;
        return Err(await func(Err()));
    }


    private static readonly SResultErr _empty = new();

    public async ValueTask<SResultErr> AndAsync(Func<ValueTask<SResultErr>> func) {
        if (Status == EResult.Err)
            return this;
        return await func();
    }

    public async ValueTask<SResult<OK>> AndAsync<OK>(Func<ValueTask<SResult<OK>>> func) {
        if (Status == EResult.Err)
            return ConvertTo<OK>();
        return await func();
    }

    public async ValueTask<SResultErr> MapAsync(Func<ValueTask> func) {
        if (Status == EResult.Ok)
            await func();
        return this;
    }

    public SResultErr Map(Action action) {
        if (Status == EResult.Ok)
            action();
        return this;
    }

    public static SResultErr Empty() {
        return _empty;
    }

    public static SResultErr Err(SErrHolder err) {
        return new SResultErr(EResult.Err, true, err);
    }

    public static SResultErr Ok() {
        return new SResultErr(EResult.Ok, true, default!);
    }

    public SResult<Ok> ToResultWithErr<Ok>() {
        if (Status == EResult.Ok)
            throw new Exception("Must Be Status Err");
        return SResult<Ok>.Err(Err());
    }

    public ResultNone ToNone() {
        return Status == EResult.Ok ? ResultNone.Ok : ResultNone.Err;
    }

    public static bool operator ==(SResultErr result, EResult status) {
        return result.Status == status;
    }

    public static bool operator !=(SResultErr result, EResult status) {
        return result.Status != status;
    }

    public static implicit operator SResultErr(ResultErr<string> v) {
        if (v == EResult.Ok) return Ok();
        return Err(v.Err());
    }

    public static implicit operator SResultErr(ResultErr<Exception> v) {
        if (v == EResult.Ok) return Ok();
        return Err(v.Err());
    }

    public SResult<OK> ConvertTo<OK>() {
        if (this == EResult.Ok)
            throw new Exception("Can Not Convert To SResult if Status Ok");

        return SResult<OK>.Err(Err());
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