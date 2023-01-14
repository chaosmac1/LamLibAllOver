namespace LamLibAllOver;

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

    public static virtual bool operator ==(IEResult result, EResult status) {
        return result.ToNone() == status;
    }

    public static virtual bool operator !=(IEResult result, EResult status) {
        return result.ToNone() != status;
    }
}

public interface IResultSwitch<TOk, EErr> {
    public ResultOpen<TOk, EErr> Unwrap();
}