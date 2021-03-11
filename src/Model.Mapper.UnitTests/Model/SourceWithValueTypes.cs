using System;

namespace Model.Mapper.Tests.Model {
    internal class SourceWithValueTypes {
        public int Integer { get; init; }
        public string String { get; init; }
        public decimal Decimal { get; init; }
        public double Double { get; init; }
        public DateTime DateTime { get; init; }
        public SomeStruct Struct { get; init; }
    }
}