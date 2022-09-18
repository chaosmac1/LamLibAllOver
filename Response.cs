namespace LamLibAllOver;

public struct Response {
    private EResponse Status;
    private bool StatusSet;

    public static EResponse ThrowByDebug(EResponse response, string? msg = null) {
#if DEBUG
        if (response == EResponse.Err)
            throw new Exception(msg);
#endif
        return response;
    }

    public static Response Empty => new();
    public static Response Err() => new() { Status = EResponse.Err, StatusSet = true };

    public static Response Ok() => new() { Status = EResponse.Ok, StatusSet = true };

    public static implicit operator EResponse(Response response) => response.Status;

    public static implicit operator Response(EResponse eResponse) => new() { Status = eResponse, StatusSet = false };

    public static bool operator ==(Response response, EResponse status) => response.Status == status;
    public static bool operator !=(Response response, EResponse status) => response.Status != status;
}

public struct ResponseErr<ERR> {
    private EResponse Status;
    private bool StatusSet;
    private ERR Value2;

    public ERR Err() {
        return Status == EResponse.Err && StatusSet
            ? Value2
            : throw new Exception("Status Not Err Or StatusSet Is False");
    }

    public static ResponseErr<ERR> Empty => new();
    public static ResponseErr<ERR> Err(ERR err) => new() { Status = EResponse.Err, StatusSet = true, Value2 = err };

    public static ResponseErr<ERR> Ok() => new() { Status = EResponse.Ok, StatusSet = true };

    public static implicit operator EResponse(ResponseErr<ERR> response) => response.Status;

    public static implicit operator ResponseErr<ERR>(EResponse eResponse) =>
        new() { Status = eResponse, StatusSet = false };

    public static explicit operator Response(ResponseErr<ERR> response) {
        if (response == EResponse.Err)
            return Response.Err();
        return Response.Ok();
    }

    public static bool operator ==(ResponseErr<ERR> response, EResponse status) => response.Status == status;
    public static bool operator !=(ResponseErr<ERR> response, EResponse status) => response.Status != status;
}

public struct Response<T> {
    public EResponse Status { get; private set; }
    private bool StatusSet;
    private T Value;

    public T Ok() {
        if (Status == EResponse.Ok && StatusSet)
            return Value;
        throw new Exception("Status Not Ok Or StatusSet Is False");
    }

    public static Response<T> Empty => new();
    public static Response<T> Err => new();

    public T? OkOrDefault() {
        if (this == EResponse.Err)
            return default;
        if (this.StatusSet == false)
            return default;
        return Value;
    }

    public static Response<T> Ok(T value) => new() { Status = EResponse.Ok, Value = value, StatusSet = true };

    public static implicit operator EResponse(Response<T> response) => response.Status;

    public static implicit operator Response<T>(T value) => Ok(value);

    public static implicit operator Response<T>(EResponse eResponse) => new() { Status = eResponse, StatusSet = false };

    public static explicit operator Response(Response<T> response) {
        if (response == EResponse.Err)
            return Response.Err();
        return Response.Ok();
    }

    public static bool operator ==(Response<T> response, EResponse status) => response.Status == status;
    public static bool operator !=(Response<T> response, EResponse status) => response.Status != status;
}

public struct Response<OK, ERR> {
    private EResponse Status;
    private bool StatusSet;
    private OK Value;
    private ERR Value2;

    public OK Ok() {
        return Status == EResponse.Ok && StatusSet
            ? Value
            : throw new Exception("Status Not Ok Or StatusSet Is False");
    }

    public ERR Err() {
        return Status == EResponse.Err && StatusSet
            ? Value2
            : throw new Exception("Status Not Err Or StatusSet Is False");
    }

    public static Response<OK, ERR> Empty => new();

    public static Response<OK, ERR> Err(ERR value) =>
        new() { Status = EResponse.Err, Value2 = value, StatusSet = true };

    public static Response<OK, ERR> Ok(OK value) => new() { Status = EResponse.Ok, Value = value, StatusSet = true };

    public static implicit operator EResponse(Response<OK, ERR> response) => response.Status;

    public static implicit operator Response<OK, ERR>(EResponse eResponse) =>
        new() { Status = eResponse, StatusSet = false };

    public static explicit operator Response(Response<OK, ERR> response) {
        if (response == EResponse.Err)
            return Response.Err();
        return Response.Ok();
    }

    public static bool operator ==(Response<OK, ERR> response, EResponse status) => response.Status == status;
    public static bool operator !=(Response<OK, ERR> response, EResponse status) => response.Status != status;
}

public enum EResponse {
    Err = 0,
    Ok = 1,
}