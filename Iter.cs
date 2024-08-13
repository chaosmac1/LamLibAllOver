using LamLibAllOver.ErrorHandling;

namespace LamLibAllOver;

public static class Iter {
    public static Result<Option<OK>, ERR> IteratorAnyResult<OK, ERR, T>(IEnumerable<T> iter,
        Func<T, Result<Option<OK>, ERR>> func) {
        foreach (var e in iter) {
            var result = func(e);
            if (result == EResult.Err)
                return result;
            var option = result.Ok();
            if (option.IsNotSet()) continue;
            return result;
        }

        return Result<Option<OK>, ERR>.Ok(Option<OK>.Empty);
    }

    public static async Task<Result<Option<OK>, ERR>> IteratorAnyResultAsync<OK, ERR, T>(IEnumerable<T> iter,
        Func<T, Task<Result<Option<OK>, ERR>>> func) {
        foreach (var e in iter) {
            var result = await func(e);
            if (result == EResult.Err)
                return result;
            var option = result.Ok();
            if (option.IsNotSet()) continue;
            return result;
        }

        return Result<Option<OK>, ERR>.Ok(Option<OK>.Empty);
    }

    public static Option<E> IteratorAnyOption<T, E>(IEnumerable<T> iter, Func<T, Option<E>> func) {
        return Option<E>.IteratorAny(iter, func);
    }

    public static async Task<Option<E>>
        IteratorAnyOptionAsync<T, E>(IEnumerable<T> iter, Func<T, Task<Option<E>>> func) {
        return await Option<E>.IteratorAnyAsync(iter, func);
    }
}