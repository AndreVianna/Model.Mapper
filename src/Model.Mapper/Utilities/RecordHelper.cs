using System;

namespace Model.Mapper {
    internal static class RecordHelpers {
        public static bool IsRecord(this Type type) =>
            type.GetMethod("<Clone>$") != null;

        public static object Clone(object original) =>
            original.GetType().GetMethod("<Clone>$")!.Invoke(original, null)!;
    }
}
