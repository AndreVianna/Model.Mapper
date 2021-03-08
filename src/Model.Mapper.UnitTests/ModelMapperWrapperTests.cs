using FluentAssertions;
using Model.Mapper.Tests.Model;
using Xunit;

namespace Model.Mapper.Tests {
    public class ModelMapperWrapperTests {
        private readonly ModelMapperWrapper _mapper;

        public ModelMapperWrapperTests() {
            _mapper = new ModelMapperWrapper();
        }

        [Fact]
        public void ModelMapperWrapper_Make_TTarget_ReturnsObjectSetter() {
            var subject = _mapper.Make<TargetClass>();
            subject.Should().BeAssignableTo<IObjectSetter>();
        }

        [Fact]
        public void ModelMapper_Make_ReturnsObjectSetter() {
            var subject = _mapper.Make(typeof(TargetClass));
            subject.Should().BeAssignableTo<IObjectSetter>();
        }

        [Fact]
        public void ModelMapper_Set_TTarget_ReturnsObjectSetter() {
            var target = new TargetClass();
            var subject = _mapper.Set(target);
            subject.Should().BeAssignableTo<IObjectSetter>();
        }

        [Fact]
        public void ModelMapper_Set_ReturnsObjectSetter() {
            var target = new TargetClass();
            var subject = _mapper.Set(typeof(TargetClass), target);
            subject.Should().BeAssignableTo<IObjectSetter>();
        }
    }
}
