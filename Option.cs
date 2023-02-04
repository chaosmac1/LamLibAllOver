namespace LamLibAllOver;

public readonly struct Option<T> {
    private readonly T _value;
    private readonly bool _isSet = false;

    public Option(T value) {
        _value = value;
        _isSet = true;
    }

    public bool IsSet() {
        return _isSet;
    }

    public T Unwrap() {
        if (!_isSet)
            throw new Exception("Try To Unwrap but Value Not Set");
        return _value;
    }

    public T Or(T value) {
        return _isSet
            ? _value
            : value;
    }

    public static Option<T> Empty => new();

    public static Option<T> With(T value) {
        return new(value);
    }

    public static implicit operator Option<T>(Result<T, object> result) {
        return result == EResult.Err
            ? Empty
            : With(result.Ok());
    }

    public static implicit operator Option<T>(ResultOk<T> result) {
        return result == EResult.Err
            ? Empty
            : With(result.Ok());
    }

    public static Option<T> NullSplit(T? value) {
        return value is null
            ? Empty
            : With(value);
    }
}