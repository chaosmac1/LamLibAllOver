namespace LamLibAllOver.ErrorHandling;

public readonly struct ResultOpen<TOk, EErr> {
    public readonly EResult Status;
    public readonly TOk? Ok;
    public readonly EErr? Err;

    internal ResultOpen(EResult status, TOk? ok, EErr? err) {
        Status = status;
        Ok = ok;
        Err = err;
    }
}