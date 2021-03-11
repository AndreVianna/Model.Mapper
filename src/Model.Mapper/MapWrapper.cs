namespace Model.Mapper {
    public class MapWrapper : IMap {
        public IMapper From(object? source) => Map.From(source);
    }
}