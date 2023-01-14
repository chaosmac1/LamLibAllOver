namespace LamLibAllOver;

public static class SqlIn {
    public static string Builder(List<string> list) {
        if (list.Count == 0) return "()";
        if (list.Count == 1) return $"('{list[0]}')";

        var builder = new StringBuilder();
        var end = list.Count - 1;

        builder.Append('(');
        for (var i = 0; i < list.Count; i++) {
            if (end == i) {
                builder.Append($"'{list[i]}'");
                builder.Append(')');
                break;
            }

            builder.Append($"'{list[i]}'");
            builder.Append(',');
        }

        return builder.ToString();
    }
}