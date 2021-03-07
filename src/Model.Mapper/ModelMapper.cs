namespace Model.Mapper {
    public class ModelMapper : IModelMapper
    {
        public IObjectSetter<T> Make<T>()
            where T: class, new() {
            return new ObjectSetter<T>();
        }

        public IObjectSetter<T> Set<T>(T target)
            where T: class, new() {
            return new ObjectSetter<T>(target);
        }
    }
}
