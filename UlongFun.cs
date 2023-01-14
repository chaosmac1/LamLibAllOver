namespace LamLibAllOver;

public static class UlongFun {
    public static ulong[] ManyByteToManyUlongs(Span<byte> bytes) {
        var res = new ulong[bytes.Length + bytes.Length % 8];

        Span<byte> byteUlong = stackalloc byte[8];
        for (var i = 0; i < res.Length; i++) {
            byteUlong.Clear();
            for (var j = i * 8; j < i * 8 + 8 && j < bytes.Length; j++) byteUlong[j - i * 8] = bytes[j];

            res[i] = BitConverter.ToUInt64(byteUlong);
        }

        return res;
    }

    public static void UlongsToByteArray(Span<ulong> input, Span<byte> res) {
        if (input.Length * 8 > res.Length)
            throw new IndexOutOfRangeException();

        for (var iInput = 0; iInput < input.Length; iInput++)
            BitConverter.TryWriteBytes(res.Slice(iInput * 8), input[iInput]);
    }
}