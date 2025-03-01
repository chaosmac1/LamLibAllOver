using LamLibAllOver.ErrorHandling.Interface;
using NLog;

namespace LamLibAllOver.ErrorHandling;

public readonly struct ResultErr<ERR> :
    IEResult,
    IGetErr<ERR>,
    IResultSwitch<object, ERR>,
    IResultError<ResultErr<ERR>, ERR> {
    private readonly EResult Status;
    private readonly bool StatusSet;
    private readonly ERR Value2;

    private ResultErr(EResult status, bool statusSet, ERR value2) {
        Status = status;
        StatusSet = statusSet;
        Value2 = value2;
    }

    public ERR Err() {
        return Status == EResult.Err && StatusSet
            ? Value2
            : throw new Exception("Status Not Err Or StatusSet Is False");
    }

    public bool Equals(EResult status) {
        return Status == status;
    }

    public ResultOpen<object, ERR> Unwrap() {
        return new ResultOpen<object, ERR>(Status, null, Value2);
    }

    public override bool Equals(object? obj) {
        return obj is EResult other && Equals(other);
    }

    public override int GetHashCode() {
        return HashCode.Combine((int)Status, StatusSet);
    }

    public ERR ErrOr(ERR value) {
        if (StatusSet || Status == EResult.Ok) return value;
        return Value2;
    }

    public ERR? ErrOrDefault() {
        if (StatusSet || Status == EResult.Ok) return default;
        return Value2;
    }

    public ResultErr<ERR> AndThen(Func<ResultErr<ERR>> func) {
        if (Status == EResult.Err)
            return this;
        return func();
    }

    public ResultErr<ERR> And(Func<ResultErr<ERR>> action) {
        if (Status == EResult.Err)
            return this;
        return action();
    }

    public ResultErr<TERR2> MapErr<TERR2>(Func<ERR, TERR2> func) {
        if (Status == EResult.Ok)
            return ResultErr<TERR2>.Ok();
        return ResultErr<TERR2>.Err(func(Err()));
    }

    public async Task<ResultErr<ERR>> AndThenAsync(Func<Task<ResultErr<ERR>>> func) {
        if (Status == EResult.Err)
            return this;
        return await func();
    }

    public async Task<Result<OK, ERR>> AndThenAsync<OK>(Func<Task<Result<OK, ERR>>> func) {
        if (Status == EResult.Err)
            return ConvertTo<OK>();
        return await func();
    }

    public async Task<ResultErr<TERR2>> MapErrAsync<TERR2>(Func<ERR, Task<TERR2>> func) {
        if (Status == EResult.Ok)
            return ResultErr<TERR2>.Ok();
        return ResultErr<TERR2>.Err(await func(Err()));
    }


    private static readonly ResultErr<ERR> _empty = new();

    public async Task<ResultErr<ERR>> AndAsync(Func<Task<ResultErr<ERR>>> func) {
        if (Status == EResult.Err)
            return this;
        return await func();
    }

    public async Task<Result<OK, ERR>> AndAsync<OK>(Func<Task<Result<OK, ERR>>> func) {
        if (Status == EResult.Err)
            return ConvertTo<OK>();
        return await func();
    }

    public async Task<ResultErr<ERR>> MapAsync(Func<Task> func) {
        if (Status == EResult.Ok)
            await func();
        return this;
    }

    public ResultErr<ERR> Map(Action action) {
        if (Status == EResult.Ok)
            action();
        return this;
    }

    public static ResultErr<ERR> Empty() {
        return _empty;
    }

    public static ResultErr<ERR> Err(ERR err) {
        return new(EResult.Err, true, err);
    }

    public static ResultErr<ERR> Ok() {
        return new(EResult.Ok, true, default!);
    }

    public Result<Ok, ERR> ToResultWithErr<Ok>() {
        if (Status == EResult.Ok)
            throw new Exception("Must Be Status Err");
        return Result<Ok, ERR>.Err(Err());
    }

    public ResultNone ToNone() {
        return Status == EResult.Ok ? ResultNone.Ok : ResultNone.Err;
    }

    public static bool operator ==(ResultErr<ERR> result, EResult status) {
        return result.Status == status;
    }

    public static bool operator !=(ResultErr<ERR> result, EResult status) {
        return result.Status != status;
    }

    public Result<OK, ERR> ConvertTo<OK>() {
        if (this == EResult.Ok)
            throw new Exception("Can Only Convert If Not OK");

        return Result<OK, ERR>.Err(Err());
    }

    public EResult Out(out ERR? err) {
        if (this == EResult.Ok) {
            err = default;
            return EResult.Ok;
        }

        err = Value2;
        return EResult.Err;
    }

    public ResultErr<ERR> LogIfError(Logger logger) {
        if (Status == EResult.Ok) return this;

        if (Err() is ILogOutput selfLogger)
            selfLogger.LogError(logger);
        else
            logger.Error(Err() is null ? "NULL" : Err()!.ToString());

        return this;
    }

    public ResultErr<ERR> LogIfError(Logger logger, string message) {
        if (Status == EResult.Ok) return this;

        if (Err() is ILogOutput selfLogger)
            selfLogger.LogError(logger, message);
        else
            logger.Error(
                (Err() is null ? "NULL" : Err()!.ToString()) + "\n\n" + message
            );

        return this;
    }

    public TRes Match<TRes>(ResultErr<ERR> self, Func<ERR, TRes> thenErr, Func<TRes> thenOk) {
        return self == EResult.Err ? thenOk() : thenErr(self.Err());
    }
}