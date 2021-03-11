using FluentAssertions;
using Model.Mapper.Tests.Model;
using Xunit;

namespace Model.Mapper.Tests {
    public class MapWrapperTests {
        private readonly MapWrapper _map;

        public MapWrapperTests() {
            _map = new MapWrapper();
        }

        [Fact]
        public void MapWrapper_From_ReturnsMapper() {
            var source = new SomeClass();
            var subject = _map.From(source);
            subject.Should().BeAssignableTo<IMapper>();
        }
    }
}
