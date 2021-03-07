using FluentAssertions;
using Xunit;

namespace Model.Mapper.Tests {
    public class ModelMapperTests
    {
        private class TestTarget {
        }

        [Fact]
        public void ModelMapper_Make_TTarget_ReturnsObjectSetter() {
            var mapper = new ModelMapper();
            var subject = mapper.Make<TestTarget>();
            subject.Should().BeAssignableTo<IObjectSetter<TestTarget>>();
        }

        [Fact]
        public void ModelMapper_Set_TTarget_ReturnsObjectSetter()
        {
            var mapper = new ModelMapper();
            var source = new TestTarget();
            var subject = mapper.Set(source);
            subject.Should().BeAssignableTo<IObjectSetter<TestTarget>>();
        }
    }
}
