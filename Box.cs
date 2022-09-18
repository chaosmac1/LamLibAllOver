namespace LamLibAllOver;

public class Box<T> where T : struct {
    public readonly T V;
    public Box(T v) => V = v;
}