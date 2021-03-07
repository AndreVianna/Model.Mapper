using System;
using FluentAssertions;
using Xunit;

namespace Model.Mapper.Tests {
    public class ObjectSetterTests {
        private class SourceClass {
            public int IntProperty { get; init; }
            public string StringProperty { get; init; }
            public decimal DecimalProperty { get; init; }
            public double DoubleProperty { get; init; }
            public DateTime DateTimeProperty { get; init; }
        }

        private class TargetClass {
            public int IntProperty { get; init; }
            public string StringProperty { get; init; }
            public decimal DecimalProperty { get; init; }
            public double DoubleProperty { get; init; }
            public DateTime DateTimeProperty { get; init; }
            public int UnaffectedProperty { get; init; }
        }

        private class InvalidTargetClass {
            public string IntProperty { get; init; }
        }

        private readonly ModelMapper _mapper;

        public ObjectSetterTests() {
            _mapper = new ModelMapper();
        }

        [Fact]
        public void ObjectSetter_From_WithSourceAndTarget_ShouldReturnTarget() {
            var target = new TargetClass();
            var source = new SourceClass();
            var subject = _mapper.Set(target).From(source);
            subject.Should().BeOfType<TargetClass>();
            subject.Should().BeSameAs(target);
        }

        [Fact]
        public void ObjectSetter_From_WithSourceAndNullTarget_ShouldReturnTarget() {
            var source = new SourceClass();
            var subject = _mapper.Make<TargetClass>().From(source);
            subject.Should().BeOfType<TargetClass>();
            subject.Should().NotBeNull();
        }

        [Fact]
        public void ObjectSetter_From_WithNullSourceAndTarget_ShouldReturnTarget() {
            var target = new TargetClass();
            var subject = _mapper.Set(target).From(default(SourceClass));
            subject.Should().BeOfType<TargetClass>();
            subject.Should().BeSameAs(target);
        }

        [Fact]
        public void ObjectSetter_From_WithNullSourceAndNullTarget_ShouldReturnNullTarget() {
            var subject = _mapper.Make<TargetClass>().From(default(SourceClass));
            subject.Should().BeNull();
        }

        [Fact]
        public void ObjectSetter_From_SourcePropertyIsAssignableToTargetProperty_ShouldBeCopied() {
            var unaffectedPropertyOriginalValue = 7;
            var target = new TargetClass {
                IntProperty = 1,
                DoubleProperty = 1.1,
                DecimalProperty = 1.1m,
                StringProperty = "Old Value.",
                DateTimeProperty = DateTime.Parse("1900-01-01"),
                UnaffectedProperty = unaffectedPropertyOriginalValue,
            };
            var source = new SourceClass {
                IntProperty = 42,
                DoubleProperty = 3.14,
                DecimalProperty = 2.17m,
                StringProperty = "Some String.",
                DateTimeProperty = DateTime.Parse("2021-02-21 22:30:00"),
            };
            var subject = _mapper.Set(target).From(source);
            subject.Should().BeOfType<TargetClass>();
            subject.Should().BeSameAs(target);
            target.IntProperty.Should().Be(source.IntProperty);
            target.DoubleProperty.Should().Be(source.DoubleProperty);
            target.DecimalProperty.Should().Be(source.DecimalProperty);
            target.StringProperty.Should().Be(source.StringProperty);
            target.DateTimeProperty.Should().Be(source.DateTimeProperty);
            target.UnaffectedProperty.Should().Be(unaffectedPropertyOriginalValue);
        }

        [Fact]
        public void ObjectSetter_From_TargetPropertyWithSameNameDifferentType_ShouldThrow() {
            var target = new InvalidTargetClass {
                IntProperty = "1",
            };
            var source = new SourceClass {
                IntProperty = 42,
                DoubleProperty = 3.14,
                DecimalProperty = 2.17m,
                StringProperty = "Some String.",
                DateTimeProperty = DateTime.Parse("2021-02-21 22:30:00"),
            };
            Action action = () => _mapper.Set(target).From(source);
            action.Should().Throw<InvalidCastException>()
                .WithMessage("Mapping error: property 'IntProperty' of type 'System.Int32' of 'Model.Mapper.Tests.ObjectSetterTests+SourceClass' can't be assigned to property 'IntProperty' of type 'System.String' of 'Model.Mapper.Tests.ObjectSetterTests+InvalidTargetClass'.");
            target.IntProperty.Should().Be(target.IntProperty);
        }
    }
}
