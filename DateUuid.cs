using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace LamLibAllOver;

[StructLayout(LayoutKind.Sequential, Pack = 0)]
public struct DateUuid : IEquatable<DateUuid>, IComparable<DateUuid> {
    private unsafe fixed byte data[16];
    private readonly string? toString;

    public DateUuid(DateTime dateTime) {
        var span = Span<byte>.Empty;
        ;
        unsafe {
            fixed (void* dataPointer = data) {
                span = new Span<byte>(dataPointer, 16);
            }
        }

        RandomNumberGenerator.Fill(span.Slice(8));
        var dataTimeLong = RevLong(dateTime.ToBinary());
        unsafe {
            var s = (byte*)&dataTimeLong;
            for (var i = 0; i < 8; i++) {
                var refS = *(s + i);
                span[i] = refS;
            }
        }
    }

    private Span<byte> GetSpan() {
        var span = Span<byte>.Empty;
        ;
        unsafe {
            fixed (void* dataPointer = data) {
                span = new Span<byte>(dataPointer, 16);
            }
        }

        return span;
    }

    public static DateUuid NewDateTime => new(DateTime.UtcNow);

    public static bool operator ==(DateUuid a, DateUuid b) {
        unsafe {
            var aPtr = (long*)a.data;
            var bPtr = (long*)b.data;

            if (aPtr[0] != bPtr[0])
                return false;
            if (aPtr[1] != bPtr[1])
                return false;
            return true;
        }
    }

    public static bool operator !=(DateUuid a, DateUuid b) {
        unsafe {
            var aPtr = (long*)a.data;
            var bPtr = (long*)b.data;

            if (aPtr[0] != bPtr[0])
                return true;
            if (aPtr[1] != bPtr[1])
                return true;
            return false;
        }
    }

    public DateTime GetDateTime() {
        unsafe {
            fixed (void* dataPointer = data) {
                return DateTime.FromBinary(RevLong(((long*)dataPointer)[0]));
            }
        }
    }

    public Guid ToGuild() {
        return new(GetSpan());
    }

    public static explicit operator Guid(DateUuid dateUuid) {
        return new(dateUuid.GetSpan());
    }

    private static long RevLong(long l) {
        var tmp = l;
        var r = 0L;

        if (tmp < 0)
            tmp *= -1;

        while (tmp > 0) {
            r = r * 10 + (tmp - tmp / 10 * 10);
            tmp = tmp / 10;
        }

        return r * (l < 0 ? -1 : 1);
    }

    public override bool Equals(object? obj) {
        return obj is DateUuid other && Equals(other);
    }

    public bool Equals(DateUuid other) {
        return this == other;
    }

    public override int GetHashCode() {
        unsafe {
            fixed (void* dataPointer = data) {
                var aPtr = (long*)dataPointer;
                return (aPtr[0] ^ aPtr[1]).GetHashCode();
            }
        }
    }

    public int CompareTo(DateUuid otherValueDateUuid) {
        Int128 self;
        Int128 other;
        unsafe {
            fixed (void* dataPointer = data) {
                var ptr = (ulong*)dataPointer;
                self = new Int128(ptr[1], ptr[0]);
            }

            var ptr2 = (ulong*)otherValueDateUuid.data;
            other = new Int128(ptr2[1], ptr2[0]);
        }

        return self.CompareTo(other);
    }

    public override string ToString() {
        if (toString is not null)
            return toString;
        var builder = new StringBuilder(36);

        var span = Span<byte>.Empty;
        unsafe {
            fixed (void* dataPointer = data) {
                span = new Span<byte>(dataPointer, 16);
            }
        }

        for (var i = 0; i < 4; i++) builder.AppendFormat("{0:x2}", span[i]);
        builder.Append('-');

        for (var i = 4; i < 6; i++) builder.AppendFormat("{0:x2}", span[i]);
        builder.Append('-');

        for (var i = 6; i < 8; i++) builder.AppendFormat("{0:x2}", span[i]);
        builder.Append('-');

        for (var i = 8; i < 10; i++) builder.AppendFormat("{0:x2}", span[i]);
        builder.Append('-');

        for (var i = 10; i < 16; i++) builder.AppendFormat("{0:x2}", span[i]);

        return builder.ToString();
    }
}