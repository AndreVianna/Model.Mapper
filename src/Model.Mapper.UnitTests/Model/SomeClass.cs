namespace Model.Mapper.Tests.Model {
    internal interface ISomeClass {
        int Property1 { get; init; }
    }

    internal class SomeClass : ISomeClass {
        public int Property1 { get; init; }
    }
}