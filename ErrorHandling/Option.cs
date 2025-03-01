namespace LamLibAllOver.ErrorHandling;

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

    public bool IsNotSet() {
        return !_isSet;
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

    public E Or<E>(E value) where E : T {
        return _isSet
            ? (E)_value!
            : value;
    }

    public T? OrNull() {
        if (_isSet)
            return _value;
        if (typeof(T).IsClass)
            return default;
        throw new NullReferenceException("Muss Be Class Type");
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

    public static Option<T> Trim(ResultOk<T> resultOk) {
        return resultOk == EResult.Ok ? With(resultOk.Ok()) : Empty;
    }

    public static Option<T> Trim(ResultOk<Option<T>> resultOk) {
        return resultOk == EResult.Ok ? resultOk.Ok() : Empty;
    }

    public static Result<Option<T>, E> ResultWrapper<E>(Result<T, E> result) {
        return result.Map(x => NullSplit(x));
    }

    public static ResultOk<Option<T>> ResultOkWrapper(ResultOk<T> result) {
        return result.Map(x => NullSplit(x));
    }

    public static Option<T> Transform<E>(Result<T, E> result) {
        return result.Unwrap() switch {
            { Ok: EResult.Ok } => NullSplit(result.Ok()),
            _ => Empty
        };
    }

    public static Option<T> Transform(ResultOk<T> result) {
        return result.Unwrap() switch {
            { Ok: EResult.Ok } => NullSplit(result.Ok()),
            _ => Empty
        };
    }

    public Option<E> Map<E>(Func<T, E> func) {
        return IsSet()
            ? new Option<E>(func(_value))
            : Option<E>.Empty;
    }

    public async Task<Option<E>> MapAsync<E>(Func<T, Task<E>> func) {
        return IsSet()
            ? new Option<E>(await func(_value))
            : Option<E>.Empty;
    }

    public static Option<T> NullSplit(T? value) {
        return value is null
            ? Empty
            : With(value);
    }

    public static Option<T> IteratorAny<E>(IEnumerable<E> iter, Func<E, Option<T>> func) {
        foreach (var e in iter) {
            var option = func(e);
            if (option.IsNotSet()) continue;
            return option;
        }

        return Empty;
    }

    public static async Task<Option<T>> IteratorAnyAsync<E>(IEnumerable<E> iter, Func<E, Task<Option<T>>> func) {
        foreach (var e in iter) {
            var option = await func(e);
            if (option.IsNotSet()) continue;
            return option;
        }

        return Empty;
    }

    public TRes Match<TRes>(Func<TRes> thenElse, Func<T, TRes> thenExist) {
        return IsSet() ? thenExist(Unwrap()) : thenElse();
    }
}