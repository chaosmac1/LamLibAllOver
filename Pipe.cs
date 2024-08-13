using LamLibAllOver.ErrorHandling;

namespace LamLibAllOver;

public struct Pipe<T, Err> where T : class {
    private readonly T? _state;
    private ResultErr<Err> _resultErr;

    private Pipe(T state) {
        _state = state;
        _resultErr = ResultErr<Err>.Ok();
    }

    private Pipe(Result<T, Err> result) {
        if (result == EResult.Err) {
            _state = null;
            _resultErr = ResultErr<Err>.Err(result.Err());
            return;
        }

        _state = result.Ok();
        _resultErr = ResultErr<Err>.Ok();
    }

    public static Pipe<T, Err> Start(T init) {
        return new(init);
    }

    public static Pipe<T, Err> Start(Result<T, Err> init) {
        return new(init);
    }

    public Pipe<T, Err> Next(Func<T, ResultErr<Err>> func) {
        if (_resultErr == EResult.Err)
            return this;

        if (_state is null)
            throw new NullReferenceException(nameof(_state));

        try {
            _resultErr = func(_state);
            return this;
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }

    public Result<T, Err> End() {
        return _resultErr == EResult.Err
            ? Result<T, Err>.Err(_resultErr.Err())
            : Result<T, Err>.Ok(_state ?? throw new NullReferenceException(nameof(_state)));
    }

    public Result<URes, Err> End<URes>(Func<T, Result<URes, Err>> func) {
        if (_resultErr == EResult.Err)
            return Result<URes, Err>.Err(_resultErr.Err());

        if (_state is null)
            throw new NullReferenceException(nameof(_state));

        try {
            return func(_state);
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }
}