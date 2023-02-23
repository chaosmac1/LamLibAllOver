using System.Diagnostics;

namespace LamLibAllOver;

public static class TraceMsg {
    public static string WithMessage(string message) {
        return $"{message}\n{new StackTrace(1, true)}";
    }
}