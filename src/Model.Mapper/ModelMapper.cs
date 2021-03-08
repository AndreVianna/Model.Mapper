using System;

namespace Model.Mapper {
    public static class ModelMapper
    {
        public static IObjectSetter Make<T>()
            where T : class {
            return Make(typeof(T));
        }

        public static IObjectSetter Make(Type targetType) {
            return new ObjectSetter(targetType);
        }

        public static IObjectSetter Set<TTarget>(TTarget? targetModel)
            where TTarget : class {
            return Set(typeof(TTarget), targetModel);
        }

        public static IObjectSetter Set(Type targetType, object? targetModel) {
            return new ObjectSetter(targetType, targetModel);
        }
    }
}
