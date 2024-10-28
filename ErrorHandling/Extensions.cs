namespace LamLibAllOver.ErrorHandling;

public static class Extensions {
    // With Option Inside
    public static TRes Match<TOk, TRes>(ResultOk<Option<TOk>> self, Func<TRes> thenErr, Func<TRes> thenNotExist,
        Func<TOk, TRes> thenExist) {
        return self == EResult.Err
            ? thenErr()
            : self.Ok().IsNotSet()
                ? thenNotExist()
                : thenExist(self.Ok().Unwrap());
    }

    public static TRes Match<TOk, TErr, TRes>(Result<Option<TOk>, TErr> self, Func<TErr, TRes> thenErr, Func<TRes> thenNotExist,
        Func<TOk, TRes> thenExist) {
        return self == EResult.Err
            ? thenErr(self.Err())
            : self.Ok().IsNotSet()
                ? thenNotExist()
                : thenExist(self.Ok().Unwrap());
    }


    public static TRes Match<TOk, TRes>(SResult<Option<TOk>> self, Func<SErrHolder, TRes> thenErr, Func<TRes> thenNotExist,
        Func<TOk, TRes> thenExist) {
        return self == EResult.Err
            ? thenErr(self.Err())
            : self.Ok().IsNotSet()
                ? thenNotExist()
                : thenExist(self.Ok().Unwrap());
    }
}