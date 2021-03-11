using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Model.Mapper {
    internal static class ObjectMapper {
        public static void Map(object source, object target) {
            var sourceType = source.GetType();
            var targetType = target.GetType();
            _ = ExecuteMap(source, sourceType, target, targetType)!;
        }

        private static object ExecuteMap(object source, Type sourceType, object? target, Type targetType) {
            if (source is ValueType) return source;
            if (source is string stringValue) return new string(stringValue);
            if (source is IDictionary dictionary) return GetUpdatedDictionary(dictionary, sourceType, target, targetType);
            if (source is IEnumerable collection) return GetUpdatedCollection(collection, sourceType, target, targetType);
            return GetUpdateReferenceType(source, sourceType, target, targetType);
        }

        private static object GetUpdatedDictionary(IDictionary dictionary, Type sourceType, object? targetValue, Type targetType) {
            var sourceValueType = sourceType.GenericTypeArguments.ToArray()[1];
            var targetKeyType = targetType.GenericTypeArguments.ToArray()[0];
            var targetValueType = targetType.GenericTypeArguments.ToArray()[1];
            var getKeyMethod = typeof(DictionaryEntry).GetMethod("get_Key")!;
            var getValueMethod = typeof(DictionaryEntry).GetMethod("get_Value")!;
            var addMethod = targetType.GetMethod("Add")!;

            targetValue = CreateOrClearDictionary(targetValue, targetType, targetKeyType, targetValueType);
            foreach (var sourceValue in dictionary) {
                var key = getKeyMethod.Invoke(sourceValue, null)!;
                var newKey = ExecuteMap(key, key.GetType(), null, targetKeyType);
                var value = getValueMethod.Invoke(sourceValue, null)!;
                var newValue = ExecuteMap(value, value.GetType(), null, targetValueType);
                addMethod.Invoke(targetValue, new[] { newKey, newValue });
            }
            return targetValue;
        }

        private static object CreateOrClearDictionary(object? dictionary, Type listType, Type keyType, Type valueType) {
            if (dictionary == null) return ObjectFactory.CreateDictionary(keyType, valueType);
            var clearMethod = listType.GetMethod("Clear")!;
            clearMethod.Invoke(dictionary, null);
            return dictionary;
        }

        private static object GetUpdatedCollection(IEnumerable collection, Type sourceType, object? targetCollection, Type targetType) {
            var sourceItemType = sourceType.GetElementType() ?? sourceType.GenericTypeArguments.ToArray()[0];
            if (targetType.IsArray) return ObjectFactory.CloneCollection(collection, sourceType, targetType);
            var targetItemType = targetType.GenericTypeArguments.ToArray()[0];
            var addMethod = targetType.GetMethod("Add")!;

            targetCollection = CreateOrClearCollection(targetCollection, targetType, targetItemType);
            foreach (var sourceItem in collection) {
                var targetItem = ExecuteMap(sourceItem, sourceItemType, null, targetItemType);
                addMethod.Invoke(targetCollection, new[] { targetItem });
            }
            return targetCollection;
        }

        private static object CreateOrClearCollection(object? collection, Type collectionType, Type itemType) {
            if (collection == null) return ObjectFactory.CreateList(itemType);
            var clearMethod = collectionType.GetMethod("Clear")!;
            clearMethod.Invoke(collection, null);
            return collection;
        }

        private static object GetUpdateReferenceType(object source, Type sourceType, object? target, Type targetType) {
            if (sourceType.IsRecord()) return RecordHelpers.Clone(source);
            target ??= CreateTargetInstance(sourceType, targetType)!;
            var targetProperties = targetType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(i => i.CanWrite).ToArray();
            foreach (var targetProperty in targetProperties) {
                var sourceProperty = sourceType.GetProperty(targetProperty.Name, BindingFlags.Instance | BindingFlags.Public);
                if (sourceProperty is null || !sourceProperty.CanRead) continue;
                var targetPropertyValue = GetNewPropertyValue(source, sourceProperty, target, targetProperty);
                targetProperty.SetValue(target, targetPropertyValue);
            }
            return target;
        }

        private static object CreateTargetInstance(Type sourceType, Type targetType) {
            if (targetType.IsInterface) return Activator.CreateInstance(sourceType)!;
            return Activator.CreateInstance(targetType)!;
        }

        private static object? GetNewPropertyValue(object source, PropertyInfo sourceProperty, object target, PropertyInfo targetProperty) {
            var sourcePropertyValue = sourceProperty.GetValue(source);
            if (sourcePropertyValue is null) return null;
            var sourcePropertyType = sourcePropertyValue.GetType();
            if (!sourcePropertyType.IsAssignableTo(targetProperty.PropertyType))
                throw new InvalidCastException($"Mapping error: {sourceProperty.Describe(sourcePropertyType)} can't be mapped to {targetProperty.Describe(targetProperty.PropertyType)}.");
            var targetPropertyValue = targetProperty.GetValue(target);
            var targetPropertyType = targetPropertyValue is null ? targetProperty.PropertyType : targetPropertyValue.GetType();
            return ExecuteMap(sourcePropertyValue, sourcePropertyType, targetPropertyValue, targetPropertyType);
        }
    }
}