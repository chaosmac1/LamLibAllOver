// ReSharper disable InconsistentNaming

namespace LamLibAllOver.ErrorHandling;

public readonly struct ResultNone : IEResult, IResultSwitch<object, object> {
    private readonly EResult Status;
    private readonly bool StatusSet;

    private ResultNone(EResult status, bool statusSet) {
        Status = status;
        StatusSet = statusSet;
    }

    public bool Equals(EResult status) {
        return Status == status;
    }

    public ResultOpen<object, object> Unwrap() {
        return new ResultOpen<object, object>(Status, null, null);
    }

    public override bool Equals(object? obj) {
        return obj is EResult other && Equals(other);
    }

    public override int GetHashCode() {
        return HashCode.Combine((int)Status, StatusSet);
    }

    public EResult ThrowIfErr(string? msg = null) {
        if (Status == EResult.Err)
            throw new Exception(msg);
        return Status;
    }

    public ResultNone And(Func<ResultNone> action) {
        if (Status == EResult.Err)
            return this;
        return action();
    }

    public async Task<ResultNone> AndAsync(Func<Task<ResultNone>> action) {
        if (Status == EResult.Err)
            return this;
        return await action();
    }

    public static ResultNone Empty = new();
    public static ResultNone Err = new(EResult.Err, true);
    public static ResultNone Ok = new(EResult.Ok, true);

    public ResultNone ToNone() {
        return this;
    }

    public static bool operator ==(ResultNone result, EResult status) {
        return result.Status == status;
    }

    public static bool operator !=(ResultNone result, EResult status) {
        return result.Status != status;
    }
}