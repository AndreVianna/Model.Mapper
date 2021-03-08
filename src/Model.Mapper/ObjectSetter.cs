using System;
using System.Linq;
using System.Reflection;

namespace Model.Mapper {
    internal class ObjectSetter : IObjectSetter {
        private readonly object? _targetModel;
        private readonly Type _targetType;

        public ObjectSetter(Type targetType, object? targetModel = null) {
            _targetModel = targetModel;
            _targetType = targetType;
        }

        public object? From(object? sourceModel) {
            if (sourceModel is null) return _targetModel;
            var targetModel = _targetModel ?? Activator.CreateInstance(_targetType)!;
            SetTargetProperties(sourceModel, targetModel);
            return targetModel;
        }

        private void SetTargetProperties(object sourceModel, object targetModel) {
            var targetProperties = _targetType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(i => i.CanWrite).ToArray();
            var sourceType = sourceModel.GetType();
            foreach (var targetProperty in targetProperties) {
                var sourceProperty = sourceType.GetProperty(targetProperty.Name, BindingFlags.Instance | BindingFlags.Public);
                if (sourceProperty is null || !sourceProperty.CanRead)
                    continue;
                if (!sourceProperty.PropertyType.IsAssignableTo(targetProperty.PropertyType))
                    throw new InvalidCastException($"Mapping error: {sourceProperty.Describe()} can't be assigned to {targetProperty.Describe()}.");
                var sourceValue = sourceProperty.GetValue(sourceModel);
                var newValue = GetNewValue(sourceValue, sourceProperty.PropertyType, targetProperty.PropertyType);
                targetProperty.SetValue(targetModel, newValue);
            }
        }

        private static object? GetNewValue(object? sourceValue, Type sourceType, Type targetType) {
            if (sourceValue == null) return default;
            if (sourceValue is ValueType) return sourceValue;
            if (sourceValue is string stringValue) return new string(stringValue);
            var cloneMethod = sourceType.GetMethod("<Clone>$");
            if (cloneMethod != null) return cloneMethod.Invoke(sourceValue, null);
            return ModelMapper.Make(targetType).From(sourceValue);
        }
    }
}
