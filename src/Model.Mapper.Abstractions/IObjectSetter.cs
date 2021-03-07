namespace Model.Mapper {
    public interface IObjectSetter<out TTarget>
        where TTarget : class, new() {
        TTarget? From<TSource>(TSource? source) where TSource : class;
    }
}