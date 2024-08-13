namespace LamLibAllOver.ErrorHandling;

public interface IGetOk<TOkValue> {
    public TOkValue Ok();
    public TOkValue OkOr(TOkValue value);
}

public interface IGetErr<TErrValue> {
    public TErrValue Err();
    public TErrValue ErrOr(TErrValue value);
}

public interface IEResult : IEquatable<EResult> {
    public ResultNone ToNone();
}

public interface IResultSwitch<TOk, EErr> {
    public ResultOpen<TOk, EErr> Unwrap();
}

public static class ResultExtensions {
    public static T OkUnwrap<T>(IGetOk<Option<T>> self) {
        return self.Ok().Unwrap();
    }

    public static bool IsOkAndIsSet<T, TSelf>(TSelf self) where TSelf : IGetOk<Option<T>>, IEResult {
        return self.ToNone() == EResult.Ok && self.Ok().IsSet();
    }
}