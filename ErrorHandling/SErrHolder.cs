using LamLibAllOver.ErrorHandling.Interface;
using NLog;

namespace LamLibAllOver.ErrorHandling;

public struct SErrHolder : ILogOutput {
    private readonly Option<ErrorDomain> _errorDomain;
    private readonly Option<Exception> _exception;
    private readonly Option<string> _str;

    internal SErrHolder(Option<Exception> exception, Option<string> str, Option<ErrorDomain> errorDomain) {
        _exception = exception;
        _str = str;
        _errorDomain = errorDomain;
    }

    public SErrHolder(Exception exception) {
        _exception = Option<Exception>.With(exception);
    }

    public SErrHolder(string str) {
        _str = Option<string>.With(str);
    }

    public SErrHolder(ErrorDomain errorDomain) {
        _errorDomain = Option<ErrorDomain>.With(errorDomain);
    }

    public static SErrHolder Parse(string? value) {
        return new SErrHolder(default, Option<string>.NullSplit(value), default);
    }

    public static SErrHolder Parse(Exception? value) {
        return new SErrHolder(Option<Exception>.NullSplit(value), default, default);
    }


    public static implicit operator SErrHolder(string? value) {
        return new SErrHolder(default, Option<string>.NullSplit(value), default);
    }

    public static implicit operator SErrHolder(Exception? value) {
        return new SErrHolder(Option<Exception>.NullSplit(value), default, default);
    }

    public static implicit operator SErrHolder(ErrorDomain value) {
        return new SErrHolder(default, default, Option<ErrorDomain>.With(value));
    }

    public bool IsException() {
        return _exception.IsSet();
    }

    public Exception? GetExceptionOrNull() {
        return _exception.OrNull();
    }

    public Option<Exception> GetException() {
        return _exception;
    }

    public bool IsErrorDomain() {
        return _errorDomain.IsSet();
    }

    public Option<ErrorDomain> GetErrorDomain() {
        return _errorDomain;
    }

    public override string ToString() {
        if (_exception.IsSet()) return _exception.Unwrap().ToString();
        if (_str.IsSet()) return _str.Unwrap();
        if (_errorDomain.IsSet()) return _errorDomain.Unwrap().ToString();
        return "";
    }

    public void LogError(Logger logger, string message) {
        if (_errorDomain.IsSet())
            _errorDomain.Unwrap().LogError(logger, message);
        else if (_exception.IsSet())
            logger.Error(_exception.Unwrap(), message);
        else if (_str.IsSet()) logger.Error(_str + "\n\n\n" + message);

        logger.Error("No Error set; " + message);
    }

    public void LogError(Logger logger) {
        if (_errorDomain.IsSet())
            _errorDomain.Unwrap().LogError(logger);
        else if (_exception.IsSet())
            logger.Error(_exception.Unwrap());
        else if (_str.IsSet()) logger.Error(_str);

        logger.Error("No Error set; ");
    }
}