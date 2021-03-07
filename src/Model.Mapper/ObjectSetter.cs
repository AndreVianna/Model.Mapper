using System;
using System.Linq;
using System.Reflection;

namespace Model.Mapper {
    internal class ObjectSetter<TTarget> : IObjectSetter<TTarget>
        where TTarget : class, new() {
        private readonly TTarget? _target;

        public ObjectSetter(TTarget? target = null) {
            _target = target;
        }

        public TTarget? From<TSource>(TSource? source)
            where TSource : class {
            if (source is null) return _target;
            var result = _target ?? new TTarget();
            SetTargetProperties(source, result);
            return result;
        }

        private static void SetTargetProperties<TSource>(TSource source, TTarget result) where TSource : class {
            var targetType = typeof(TTarget);
            var targetProperties = targetType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(i => i.CanWrite).ToArray();
            var sourceType = typeof(TSource);
            foreach (var targetProperty in targetProperties) {
                var sourceProperty = sourceType.GetProperty(targetProperty.Name, BindingFlags.Instance | BindingFlags.Public);
                if (sourceProperty is null || !sourceProperty.CanRead) 
                    continue;
                if (!sourceProperty.PropertyType.IsAssignableTo(targetProperty.PropertyType))
                    throw new InvalidCastException($"Mapping error: {sourceProperty.Describe()} can't be assigned to {targetProperty.Describe()}.");
                targetProperty.SetValue(result, sourceProperty.GetValue(source));
            }
        }
    }
}
