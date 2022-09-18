namespace LamLibAllOver;

public static class Builder {
    public static Thread ThreadStart(ThreadStart start) {
        var res = new Thread(start, 8000000);
        res.Start();
        return res;
    }

    public static Thread ThreadStart(ThreadStart start, int maxStackSize) {
        var res = new Thread(start, maxStackSize);
        res.Start();
        return res;
    }

    public static Thread ThreadStart(ParameterizedThreadStart start, int maxStackSize) {
        var res = new Thread(start, maxStackSize);
        res.Start();
        return res;
    }

    public static Thread ThreadStart(ParameterizedThreadStart start) {
        var res = new Thread(start);
        res.Start();
        return res;
    }
}