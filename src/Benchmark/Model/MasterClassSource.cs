using System.Collections.Generic;

namespace Benchmark.Model {
    public class MasterClassSource {
        public int Integer { get; set; }
        public string String { get; set; }
        public decimal? Decimal { get; set; }
        public DetailClassSource Detail1 { get; set; } = new ();
        public DetailClassSource Detail2 { get; set; } = new ();
        public DetailClassSource[] DetailArray { get; set; } = { new (), new () };
        public ICollection<DetailClassSource> Details { get; set; } = new List<DetailClassSource> { new(), new() };
    }
}