using System.Diagnostics;

namespace LamLibAllOver.ErrorHandling;

public static class ResultStack {
    public static ResultErr<string> RErrMsgErr(string message) {
        return ResultErr<string>.Err($"{message}\n{new StackTrace(1, true)}");
    }

    public static Result<Ok, string> RMsgErr<Ok>(string message) {
        return Result<Ok, string>.Err($"{message}\n{new StackTrace(1, true)}");
    }

    public static SResultErr SRErrMsgErr(string message) {
        return SResultErr.Err($"{message}\n{new StackTrace(1, true)}");
    }

    public static SResult<Ok> SRMsgErr<Ok>(string message) {
        return SResult<Ok>.Err($"{message}\n{new StackTrace(1, true)}");
    }
}