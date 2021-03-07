namespace Model.Mapper {
    public interface IModelMapper {
        IObjectSetter<TTarget> Make<TTarget>() where TTarget : class, new();
        IObjectSetter<T> Set<T>(T target) where T : class, new();
    }
}
