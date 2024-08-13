using NLog;

namespace LamLibAllOver.ErrorHandling.Interface;

public interface ILogOutput {
    public void LogError(Logger logger, string message);
    public void LogError(Logger logger);
}