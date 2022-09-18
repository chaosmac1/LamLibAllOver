namespace LamLibAllOver;

public static class Time {
    public static string ToScyllaString(DateTime dateTime)
        => dateTime.ToString("yyyy-MM-ddTHH\\:mm\\:ss");

    public static DateTime FromScyllaString(string time) {
        return DateTime.Parse(time);
    }

    public static DateTime JavaTimeStampToDateTime(double javaTimeStamp) {
        // Java timestamp is milliseconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddMilliseconds(javaTimeStamp).ToLocalTime();
        return dateTime;
    }

    public static DateTime UnixTimeStampToDateTime(double unixTimeStamp) {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dateTime;
    }

    public static long DateTimeToJavaTimeStamp(DateTime value) {
        // Java timestamp is milliseconds past epoch

        var span = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        return (value.Ticks - span.Ticks) / 10000;
    }
}