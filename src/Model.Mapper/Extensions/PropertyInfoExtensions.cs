using System.Reflection;

namespace Model.Mapper {
    internal static class PropertyInfoExtensions {
        internal static string Describe(this PropertyInfo property) {
            return $"property '{property.Name}' of type '{property.PropertyType}' of '{property.DeclaringType}'";
        }
    }
}
