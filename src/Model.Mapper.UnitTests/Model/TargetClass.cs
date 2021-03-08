using System;
using System.Collections.Generic;

namespace Model.Mapper.Tests.Model {
    internal class TargetClass {
        //Value Types
        public int Property1 { get; init; }
        public double Property2 { get; init; }
        public decimal Property3 { get; init; }
        public DateTime Property4 { get; init; }

        //Value Type Special Cases
        public string Property5 { get; init; }
        public SomeStruct Property6 { get; init; }

        //Reference Types
        public SomeRecord Property7 { get; init; }
        public SomeClass Property8 { get; init; }
        public ISomeClass Property9 { get; init; }

        //Collections
        public IEnumerable<int> Property10 { get; init; }
        public IEnumerable<string> Property11 { get; init; }
        public IEnumerable<SomeStruct> Property12 { get; init; }
        public IEnumerable<SomeRecord> Property13 { get; init; }
        public IEnumerable<SomeClass> Property14 { get; init; }

        //Collections
        public int[] Property15 { get; init; }
        public List<int> Property16 { get; init; }

        public int Unmapped1 { get; init; }
        public SomeClass Unmapped2 { get; init; }
    }
}