using System.Collections.Generic;

namespace Model.Mapper.Tests.Model {
    internal class SourceWithCollections {
        public int[] Array { get; init; }

        public List<int> List { get; init; }
        public HashSet<int> HashSet { get; init; }
        public Dictionary<int, string> Dictionary { get; init; }

        public IEnumerable<int> EnumerableInterface { get; init; }
        public ICollection<int> CollectionInterface { get; init; }
        public IList<int> ListInterface { get; init; }
        public ISet<int> SetInterface { get; init; }
        public IDictionary<int, string> DictionaryInterface { get; init; }

        public IEnumerable<SomeClass> Classes { get; init; }
        public IDictionary<string, SomeClass> Index { get; init; }
    }
}