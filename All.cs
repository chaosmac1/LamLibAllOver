namespace LamLibAllOver;

public static class All {
    public static T OutTwo<T>(T value, out T outValue) {
        outValue = value;
        return value;
    }
}