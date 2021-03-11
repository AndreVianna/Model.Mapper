using System;
using System.Collections;
using static Model.Mapper.ObjectFactory;

namespace Model.Mapper {
    internal class Mapper : IMapper {
        private readonly object _source;
        private readonly Type _sourceType;

        public Mapper(object source) {
            _source = source;
            _sourceType = source.GetType();
        }

        public TTarget To<TTarget>() => (TTarget)To(typeof(TTarget));

        public object To(Type targetType) {
            if (_sourceType.IsAssignableTo(targetType)) return CloneObject(_source);
            if (targetType.IsInterface) throw new ArgumentException("Cannot create instance of an interface.");
            if (targetType.IsArray && _source is IEnumerable collection) return CloneCollection(collection, _sourceType, targetType);
            var target = CreateInstanceOf(targetType);
            To(target);
            return target;
        }

        public void To(object target) {
            ObjectMapper.Map(_source, target);
        }
    }
}