using System.Diagnostics;

namespace LamLibAllOver;

public static class ResultStack {
    public static ResultErr<string> RErrMsgErr(string message) {
        return ResultErr<string>.Err($"{message}\n{new StackTrace(1, true)}");
    }

    public static Result<Ok, string> RMsgErr<Ok>(string message) {
        return Result<Ok, string>.Err($"{message}\n{new StackTrace(1, true)}");
    }
}