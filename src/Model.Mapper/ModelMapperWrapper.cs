using System;

namespace Model.Mapper {
    public class ModelMapperWrapper : IModelMapper {
        public IObjectSetter Make<T>()
            where T : class {
            return ModelMapper.Make<T>();
        }

        public IObjectSetter Make(Type targetType) {
            return ModelMapper.Make(targetType);
        }

        public IObjectSetter Set<TTarget>(TTarget? targetModel)
            where TTarget : class {
            return ModelMapper.Set(targetModel);
        }

        public IObjectSetter Set(Type targetType, object? targetModel) {
            return ModelMapper.Set(targetType, targetModel);
        }
    }
}