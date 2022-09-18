#nullable enable

namespace LamLibAllOver.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class VersionAttri : Attribute {
    public sealed class Version {
        public ushort IsMajor { get; init; }
        public ushort IsMinor { get; init; }
        public ushort IsPatch { get; init; }
        public ushort WorkMinMajor { get; init; }
        public ushort WorkMinMinor { get; init; }
        public ushort WorkMinPatch { get; init; }

        public override bool Equals(object? obj) {
            return obj is Version other && Equals(other);
        }

        public override int GetHashCode() {
            return HashCode.Combine(IsMajor, IsMinor, IsPatch, WorkMinMajor, WorkMinMinor, WorkMinPatch);
        }

        public Version() {
        }

        public Version(Version version) {
            IsMajor = version.IsMajor;
            IsMinor = version.IsMinor;
            IsPatch = version.IsPatch;
            WorkMinMajor = version.WorkMinMajor;
            WorkMinMinor = version.WorkMinMinor;
            WorkMinPatch = version.WorkMinPatch;
        }

        public Version(ushort isMajor, ushort isMinor, ushort isPatch, ushort workMinMajor, ushort workMinMinor,
            ushort workMinPatch) {
            IsMajor = isMajor;
            IsMinor = isMinor;
            IsPatch = isPatch;
            WorkMinMajor = workMinMajor;
            WorkMinMinor = workMinMinor;
            WorkMinPatch = workMinPatch;
        }

        public (ushort, ushort, ushort, ushort, ushort, ushort) GetTuple() => (
            IsMajor, IsMinor, IsPatch, WorkMinMajor, WorkMinMinor, WorkMinPatch);

        public bool Equals(Version other) {
            return IsMajor == other.IsMajor && IsMinor == other.IsMinor && IsPatch == other.IsPatch &&
                   WorkMinMajor == other.WorkMinMajor && WorkMinMinor == other.WorkMinMinor &&
                   WorkMinPatch == other.WorkMinPatch;
        }

        public static bool operator <(Version a, Version b) {
            if (a.IsMajor < b.IsMajor) return true;
            if (a.IsMinor < b.IsMinor) return true;
            if (a.IsPatch < b.IsPatch) return true;
            if (a.WorkMinMajor < b.WorkMinMajor) return true;
            if (a.WorkMinMinor < b.WorkMinMinor) return true;
            if (a.WorkMinPatch < b.WorkMinPatch) return true;
            return false;
        }

        public static bool operator >(Version a, Version b) {
            if (a.IsMajor > b.IsMajor) return true;
            if (a.IsMinor > b.IsMinor) return true;
            if (a.IsPatch > b.IsPatch) return true;
            if (a.WorkMinMajor > b.WorkMinMajor) return true;
            if (a.WorkMinMinor > b.WorkMinMinor) return true;
            if (a.WorkMinPatch > b.WorkMinPatch) return true;
            return false;
        }

        public static bool operator ==(Version a, Version b) {
            if (a.IsMajor != b.IsMajor) return false;
            if (a.IsMinor != b.IsMinor) return false;
            if (a.IsPatch != b.IsPatch) return false;
            if (a.WorkMinMajor != b.WorkMinMajor) return false;
            if (a.WorkMinMinor != b.WorkMinMinor) return false;
            if (a.WorkMinPatch != b.WorkMinPatch) return false;
            return true;
        }

        public static bool operator !=(Version a, Version b) {
            if (a.IsMajor == b.IsMajor) return false;
            if (a.IsMinor == b.IsMinor) return false;
            if (a.IsPatch == b.IsPatch) return false;
            if (a.WorkMinMajor == b.WorkMinMajor) return false;
            if (a.WorkMinMinor == b.WorkMinMinor) return false;
            if (a.WorkMinPatch == b.WorkMinPatch) return false;
            return true;
        }

        public override string ToString() => "Version" + IsMajor + "." + IsMinor + "." + IsPatch + "." + WorkMinMajor +
                                             "." + WorkMinMinor + "." + WorkMinPatch;
    }

    public VersionAttri(ushort isMajor, ushort isMinor, ushort isPatch, ushort workMinMajor, ushort workMinMinor,
        ushort workMinPatch) {
        Ver = new Version() {
            IsMajor = isMajor,
            IsMinor = isMinor,
            IsPatch = isPatch,
            WorkMinMajor = workMinMajor,
            WorkMinMinor = workMinMinor,
            WorkMinPatch = workMinPatch
        };
    }

    private readonly Version Ver;

    public Version GetVersion() => new(Ver);

    public override string ToString() => Ver.ToString();
}