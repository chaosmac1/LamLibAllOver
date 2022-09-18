#nullable enable

namespace LamLibAllOver.ExtensionMethods;

public static class MyExtensions {
    public static string AddSpaceLeft(this String str, int count) => new String(' ', count) + str;

    public static bool SingleEq(this List<int> list, int value) {
        foreach (var i in list) {
            if (i == value) return true;
        }

        return false;
    }

    public static int CoutOfGTE(this List<int> list, int value) {
        int res = 0;

        foreach (var i in list) {
            if (i >= value) res++;
        }

        return res;
    }

    public static int CoutOfLTE(this List<int> list, int value) {
        int res = 0;
        foreach (var i in list) {
            if (i <= value) res++;
        }

        return res;
    }
}