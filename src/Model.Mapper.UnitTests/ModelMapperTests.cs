using FluentAssertions;
using Xunit;

namespace Model.Mapper.Tests {
    public class ModelMapperTests {
        private class TargetClass {
        }

        [Fact]
        public void ModelMapper_Make_TTarget_ReturnsObjectSetter() {
            var subject = ModelMapper.Make<TargetClass>();
            subject.Should().BeAssignableTo<IObjectSetter>();
        }

        [Fact]
        public void ModelMapper_Make_ReturnsObjectSetter() {
            var subject = ModelMapper.Make(typeof(TargetClass));
            subject.Should().BeAssignableTo<IObjectSetter>();
        }

        [Fact]
        public void ModelMapper_Set_TTarget_ReturnsObjectSetter()
        {
            var target = new TargetClass();
            var subject = ModelMapper.Set(target);
            subject.Should().BeAssignableTo<IObjectSetter>();
        }

        [Fact]
        public void ModelMapper_Set_ReturnsObjectSetter() {
            var target = new TargetClass();
            var subject = ModelMapper.Set(typeof(TargetClass), target);
            subject.Should().BeAssignableTo<IObjectSetter>();
        }
    }
}
