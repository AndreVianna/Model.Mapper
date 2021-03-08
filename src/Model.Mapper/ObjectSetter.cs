using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Model.Mapper {
    internal class ObjectSetter : IObjectSetter {
        private object? _targetModel;
        private readonly Type _targetType;
        private object _sourceModel;
        private Type _sourceType;

        public ObjectSetter(Type targetType, object? targetModel = null) {
            _targetModel = targetModel;
            _targetType = targetType;
        }

        public object? From(object? sourceModel) {
            if (sourceModel is null) return _targetModel;
            if (sourceModel is ValueType || sourceModel is string) return sourceModel;
            _sourceModel = sourceModel;
            _sourceType = _sourceModel.GetType();
            UpdateTargetModel();
            return _targetModel;
        }

        private void UpdateTargetModel() {
            _targetModel ??= CreateTargetModel();
            UpdateTargetModelProperties();
        }

        private object CreateTargetModel() {
            var resultType = (_targetType.IsInterface && _sourceType.IsAssignableTo(_targetType)) ? _sourceType : _targetType;
            return Activator.CreateInstance(resultType)!;
        }

        private void UpdateTargetModelProperties() {
            var targetProperties = _targetType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(i => i.CanWrite).ToArray();
            foreach (var targetProperty in targetProperties) {
                var sourceProperty = _sourceType.GetProperty(targetProperty.Name, BindingFlags.Instance | BindingFlags.Public);
                if (sourceProperty is null || !sourceProperty.CanRead)
                    continue;
                if (!sourceProperty.PropertyType.IsAssignableTo(targetProperty.PropertyType))
                    throw new InvalidCastException($"Mapping error: {sourceProperty.Describe()} can't be assigned to {targetProperty.Describe()}.");
                var newValue = CloneSourceModelProperty(_sourceModel, sourceProperty, _targetModel, targetProperty);
                targetProperty.SetValue(_targetModel, newValue);
            }
        }

        private static object? CloneSourceModelProperty(object sourceModel, PropertyInfo sourceProperty, object? targetModel, PropertyInfo targetProperty) {
            var sourcePropertyValue = sourceProperty.GetValue(sourceModel);
            if (sourcePropertyValue == null) return default;
            var targetPropertyValue = targetProperty.GetValue(targetModel);
            if (sourcePropertyValue is ValueType) return sourcePropertyValue;
            if (sourcePropertyValue is string stringValue) return new string(stringValue);
            if (sourcePropertyValue is ICloneable cloneable) return cloneable.Clone();
            if (sourcePropertyValue is IEnumerable collection) return CloneCollection(collection, targetProperty.PropertyType);
            return CloneRecord(sourcePropertyValue, sourceProperty.PropertyType, targetPropertyValue, targetProperty.PropertyType);
        }

        private static object? CloneCollection(IEnumerable sourceValues, Type targetType) {
            var itemType = targetType.GenericTypeArguments.ToArray()[0];
            var result = Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType))!;
            var addMethod = result.GetType().GetMethod("Add")!;
            foreach (var sourceValue in sourceValues) {
                var newValue = ModelMapper.Make(itemType).From(sourceValue);
                addMethod.Invoke(result, new[] { newValue });
            }
            return result;
        }

        private static object? CloneRecord(object sourceValue, Type sourceType, object? targetValue, Type targetType) {
            var cloneMethod = sourceType.GetMethod("<Clone>$");
            if (cloneMethod != null) return cloneMethod.Invoke(sourceValue, null);
            return ModelMapper.Set(targetType, targetValue).From(sourceValue);
        }
    }
}
