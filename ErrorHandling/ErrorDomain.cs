using System.Diagnostics;
using LamLibAllOver.ErrorHandling.Interface;
using NLog;

namespace LamLibAllOver.ErrorHandling;

public struct ErrorDomain : ILogOutput {
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
        return AddExtra(new StackTrace(1, true).ToString());
    }

    public ErrorDomain AddException(Exception exception) {
        return AddExtra(exception);
    }

    public ErrorDomain AddExtra(object obj) {
        var self = this;
        self.Extra = obj;
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

    public void LogError(Logger logger, string message) {
        var extraString = Extra is null ? string.Empty : $"Extra: {Extra};;";

        logger.Error(
            """
            HumanMessage: {};;

            Message: {};;

            Domain: {};;

            {}
            """, HumanMessage, Message, Domain, extraString + "\n\n" + message
        );
    }

    public void LogError(Logger logger) {
        var extraString = Extra is null ? string.Empty : $"Extra: {Extra};;";

        logger.Error(
            """
            HumanMessage: {};;

            Message: {};;

            Domain: {};;

            {}
            """, HumanMessage, Message, Domain, extraString
        );
    }
}