namespace LamLibAllOver;

public class TryCatch {
    public void VoidAction(Action action) {
        try {
            action();
        }
        catch (Exception) {
            // ignored
        }
    }

    public Result<T, Exception> FuncWithT<T>(Func<T> func) {
        try {
            return Result<T, Exception>.Ok(func());
        }
        catch (Exception e) {
            return Result<T, Exception>.Err(e);
        }
    }
}