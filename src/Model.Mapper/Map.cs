using System;

namespace Model.Mapper {
    public static class Map
    {
        public static IMapper From(object? source) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            return new Mapper(source);
        }
    }
}
