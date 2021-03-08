using System;

namespace Model.Mapper {
    public interface IModelMapper {
        IObjectSetter Make(Type targetType);
        IObjectSetter Make<TTarget>() where TTarget : class;
        IObjectSetter Set(Type targetType, object? targetModel);
        IObjectSetter Set<TTarget>(TTarget? target) where TTarget : class;
    }
}
