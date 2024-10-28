namespace LamLibAllOver.ErrorHandling;

public static class ExtensionsMatch {
    public static T Match<T>(ResultNone self, Func<T> thenErr, Func<T> thenOk) {
        return self == EResult.Err ? thenOk() : thenErr();
    }

    public static TRes Match<TErr, TRes>(ResultErr<TErr> self, Func<TErr, TRes> thenErr, Func<TRes> thenOk) {
        return self == EResult.Err ? thenOk() : thenErr(self.Err());
    }

    public static TRes Match<TOk, TRes>(ResultOk<TOk> self, Func<TRes> thenErr, Func<TOk, TRes> thenOk) {
        return self == EResult.Err ? thenOk(self.Ok()) : thenErr();
    }

    public static TRes Match<TOk, TErr, TRes>(Result<TOk, TErr> self, Func<TErr, TRes> thenErr, Func<TOk, TRes> thenOk) {
        return self == EResult.Err ? thenOk(self.Ok()) : thenErr(self.Err());
    }


    public static TRes Match<TRes>(SResultErr self, Func<SErrHolder, TRes> thenErr, Func<TRes> thenOk) {
        return self == EResult.Err ? thenOk() : thenErr(self.Err());
    }

    public static TRes Match<TOk, TRes>(SResult<TOk> self, Func<SErrHolder, TRes> thenErr, Func<TOk, TRes> thenOk) {
        return self == EResult.Err ? thenOk(self.Ok()) : thenErr(self.Err());
    }

    public static TRes Match<TValue, TRes>(Option<TValue> self, Func<TRes> thenElse, Func<TValue, TRes> thenExist) {
        return self.IsSet() ? thenExist(self.Unwrap()) : thenElse();
    }

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