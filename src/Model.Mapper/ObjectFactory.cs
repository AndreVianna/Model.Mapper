using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Model.Mapper {
    internal static class ObjectFactory {
        public static object CloneObject(object source) {
            var sourceType = source.GetType();
            var cloneMethod = sourceType.GetRuntimeMethods().FirstOrDefault(i => i.Name == "MemberwiseClone")!;
            return cloneMethod.Invoke(source, null)!;
        }

        public static object CloneCollection(IEnumerable source, Type sourceType, Type targetType) {
            var targetItemType = targetType.GetElementType()!;
            var countMethod = sourceType.GetRuntimeMethod("get_Count", Array.Empty<Type>()) ?? sourceType.GetRuntimeMethod("get_Length", Array.Empty<Type>())!;
            var count = (int)countMethod.Invoke(source, null)!;
            var result = (IList)CreateEmptyArray(targetItemType, count);
            var index = 0;
            foreach (var sourceItem in source) {
                result[index++] = Map.From(sourceItem).To(targetItemType);
            }
            return result;
        }

        public static object CreateInstanceOf(Type targetType) {
            if (targetType.IsAssignableTo(typeof(IDictionary))) return CreateDictionary(targetType);
            if (targetType.IsAssignableTo(typeof(IEnumerable))) return CreateCollection(targetType);
            return Activator.CreateInstance(targetType)!;
        }

        private static object CreateDictionary(Type targetType) {
            var keyType = targetType.GenericTypeArguments.ToArray()[0];
            var valueType = targetType.GenericTypeArguments.ToArray()[1];
            return CreateDictionary(keyType, valueType);
        }

        public static object CreateDictionary(Type keyType, Type valueType) {
            return Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(keyType, valueType))!;
        }

        private static object CreateCollection(Type targetType) {
            var itemType = targetType.GenericTypeArguments.ToArray()[0];
            return CreateList(itemType);
        }

        public static object CreateList(Type itemType) {
            return Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType))!;
        }

        private static object CreateEmptyArray(Type itemType, int size = 0) {
            return Array.CreateInstance(itemType, size);
        }
    }
}