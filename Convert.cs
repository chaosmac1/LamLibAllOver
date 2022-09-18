#nullable enable

namespace LamLibAllOver.Conf;

public static class Convert {
    public static byte[] StringToByteArray(string str) => new UTF8Encoding().GetBytes(str);

    public static string ByteArrayToString(byte[] bytes) => new UTF8Encoding().GetString(bytes);

    public static byte[] Base64Decode(string inputStrBase64)
        => System.Convert.FromBase64CharArray(inputStrBase64.ToCharArray(), 0, inputStrBase64.Length);

    public static string Base64DecodeString(string inputStrBase64) => ByteArrayToString(Base64Decode(inputStrBase64));

    public static string Base64EncoderString(string inputStr)
        => System.Convert.ToBase64String(StringToByteArray(inputStr));

    public static byte[] ReadFully(Stream input) {
        byte[] buffer = new byte[16 * 1024];
        using (MemoryStream ms = new MemoryStream()) {
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0) {
                ms.Write(buffer, 0, read);
            }

            return ms.ToArray();
        }
    }
}