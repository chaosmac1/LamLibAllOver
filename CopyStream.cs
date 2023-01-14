namespace LamLibAllOver;

public static class CopyStream {
    public static void Move(Stream input, Stream output) {
        var buffer = new byte[32768];
        Span<byte> span = new(buffer);
        int read;
        while ((read = input.Read(span)) > 0) output.Write(span.Slice(0, read));
    }

    public static void Move(FileStream input, Stream output) {
        var buffer = new byte[32768];
        Span<byte> span = new(buffer);
        int read;
        while ((read = input.Read(span)) > 0) output.Write(span.Slice(0, read));
    }
}