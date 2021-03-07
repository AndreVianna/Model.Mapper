using System.Collections.Generic;

namespace Benchmark.Model {
    public class MasterClassTarget {
        public int Integer { get; set; }
        public string String { get; set; }
        public decimal? Decimal { get; set; }
        public DetailClassSource Detail1 { get; set; }
        public DetailClassSource Detail2 { get; set; }
        public DetailClassSource[] DetailArray { get; set; }
        public ICollection<DetailClassSource> Details { get; set; }
    }
}