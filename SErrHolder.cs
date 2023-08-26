namespace LamLibAllOver;

public struct SErrHolder {
    internal Exception? _exception;
    internal string? _str;

    internal SErrHolder(Exception? exception, string? str) {
        _exception = exception;
        _str = str;
    }

    public SErrHolder(Exception exception) {
        _exception = exception;
    }

    public SErrHolder(string str) {
        _str = str;
    }

    public static SErrHolder Parse(string? value) {
        return new SErrHolder(null, value ?? "");
    }

    public static SErrHolder Parse(Exception? value) {
        return new SErrHolder(value, value is null ? "" : null);
    }


    public static implicit operator SErrHolder(string? value) {
        return new SErrHolder(null, value ?? "");
    }

    public static implicit operator SErrHolder(Exception? value) {
        return new SErrHolder(value, value is null ? "" : null);
    }

    public override string ToString() {
        if (_exception is not null) return _exception.ToString();
        if (_str is not null) return _str;
        return "";
    }
}