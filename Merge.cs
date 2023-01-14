namespace LamLibAllOver;

public static class Merge {
    public static string ObjectsToString(Span<object> span) {
        var builder = new StringBuilder(span.Length * 32);
        foreach (var o in span) builder.Append(o);

        return builder.ToString();
    }

    public static string ListToString<T>(IReadOnlyList<T> list) {
        var builder = new StringBuilder(list.Count * 32);

        foreach (var x1 in list) builder.Append(x1);

        return builder.ToString();
    }

    public static string ObjectsToString(object[] objs) {
        var span = new Span<object>(objs);
        return ObjectsToString(span);
    }
}