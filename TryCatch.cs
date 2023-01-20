namespace LamLibAllOver;

public static class TryCatch {
    public static ResultErr<Exception> VoidAction(Action action) {
        try {
            action();
            return ResultErr<Exception>.Ok();
        }
        catch (Exception e) {
            return ResultErr<Exception>.Err(e);
        }
    }

    public static Result<T, Exception> FuncWithT<T>(Func<T> func) {
        try {
            return Result<T, Exception>.Ok(func());
        }
        catch (Exception e) {
            return Result<T, Exception>.Err(e);
        }
    }
}