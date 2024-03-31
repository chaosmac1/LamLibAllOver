using System.Diagnostics;

namespace LamLibAllOver;

public struct ErrorDomain {
    public string HumanMessage { get; }
    public string Message { get; }
    public string Domain { get; }
    public object? Extra { get; private set; }

    public ErrorDomain(string humanMessage, string message, string domain, object extra) {
        HumanMessage = humanMessage;
        Message = message;
        Domain = domain;
        Extra = extra;
    }

    public ErrorDomain(string humanMessage, string message, string domain) {
        HumanMessage = humanMessage;
        Message = message;
        Domain = domain;
        Extra = null;
    }

    public ErrorDomain AddStackTrace() {
        var self = this;
        self.Extra = new StackTrace(1, true).ToString();
        return self;
    }


    public override string ToString() {
        var extraString = Extra is null ? string.Empty : $"Extra: {Extra};;";

        return
            $"""
             HumanMessage: {HumanMessage};;

             Message: {Message};;

             Domain: {Domain};;

             {extraString}
             """;
    }
}