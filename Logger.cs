using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace LamLibAllOver;

public static class Logger {
    private const string Path = "./log";

    public static void Write(string text) {
        var f = !File.Exists(Path)
            ? File.CreateText(Path)
            : File.AppendText(Path);
        f.WriteLine(text);
        f.Close();
    }

    public record ErrorMsgParas(string Msg, int LineNumber, string Path);

    public static ErrorMsgParas ErrorMsgFac(string msg, [CallerLineNumber] int lineNumber = 0,
        [CallerFilePath] string path = null) => new ErrorMsgParas(Msg: msg, lineNumber, path);

    public static void ErrorMsg(ErrorMsgParas paras) {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("Error MSG:");
        Console.ResetColor();
        Console.WriteLine($" {paras.Msg} \n");
        Console.ForegroundColor = ConsoleColor.Red;
        StackTrace stackTrace = new StackTrace(0, true);
        foreach (var i in stackTrace.GetFrames()) {
            Console.ResetColor();
            Console.Write($"at {i.GetMethod().ToString()} in ");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write($"{i.GetFileName()}:line {i.GetFileLineNumber()}\n");
        }

        Console.ResetColor();
    }

    public static void ErrorMsg(string msg, [CallerLineNumber] int lineNumber = 0,
        [CallerFilePath] string path = null) {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("Error MSG:");
        Console.ResetColor();
        Console.WriteLine($" {msg} \n");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Line: {lineNumber} \nPATH: {path}");
        Console.ResetColor();
    }

    private static Queue<(string msgName, string msgStatus, string? extra)> QInfoMsg = new();
    private static Task _task = CreateInfoPrinterTask();

    private static Task CreateInfoPrinterTask() {
        var t = new Task(InfoPrinter);
        t.Start();
        return t;
    }

    private static void InfoPrinter() {
        while (true) {
            while (QInfoMsg.Count != 0) {
                var i = QInfoMsg.Dequeue();
                Console.ResetColor();
                Console.Write(i.msgName);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(i.msgStatus);
                Console.ResetColor();
                if (i.extra is not null) Console.Write(i.extra);
                Console.Write("\n");
                Thread.Sleep(10);
            }

            Thread.Sleep(100);
        }
    }

    public static void InfoMsg(string msgName, string msgStatus, string? extra = null) {
        QInfoMsg.Enqueue((msgName, msgStatus, extra));
    }

    public static void PrintInGreen(string msg) {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(msg);
        Console.ResetColor();
    }

    public static void PrintInRed(string msg) {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(msg);
        Console.ResetColor();
    }
}