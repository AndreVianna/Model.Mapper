using System;
using System.Reflection;

namespace Model.Mapper {
    internal static class PropertyInfoExtensions {
        internal static string Describe(this PropertyInfo property, Type type) {
            return $"property '{property.Name}' of type '{type}' of '{property.DeclaringType}'";
        }
    }
}
