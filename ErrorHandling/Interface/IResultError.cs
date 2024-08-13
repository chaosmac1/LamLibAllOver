using NLog;

namespace LamLibAllOver.ErrorHandling.Interface;

public interface IResultError<Result, TErr> where Result : IEResult, IGetErr<TErr> {
    public Result LogIfError(Logger logger);
    public Result LogIfError(Logger logger, string message);
}