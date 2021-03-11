using System;

namespace Model.Mapper {
    public interface IMapper {
        TTarget To<TTarget>();
        object To(Type targetType);
        void To(object target);
    }
}