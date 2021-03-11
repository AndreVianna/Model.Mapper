namespace Model.Mapper.Tests.Model {
    internal class TargetWithReferenceTypes {
        public SomeRecord Record { get; init; }
        public SomeClass Class { get; init; }
        public ISomeClass Interface { get; init; }
        public SomeClass NullClass { get; init; }
        public ISomeClass NullInterface { get; init; }
        public SomeClass NullableClass { get; init; }
    }
}