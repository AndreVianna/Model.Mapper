using System;
using System.Collections.Generic;
using FluentAssertions;
using Model.Mapper.Tests.Model;
using Xunit;

namespace Model.Mapper.Tests {
    public class ObjectSetterTests {
        [Fact]
        public void ObjectSetter_From_WithSourceAndTarget_ShouldReturnTarget() {
            var target = new TargetClass();
            var source = new SourceClass();
            var subject = ModelMapper.Set(target).From(source);
            subject.Should().BeOfType<TargetClass>();
            subject.Should().BeSameAs(target);
        }

        [Fact]
        public void ObjectSetter_From_WithSourceAndNullTarget_ShouldReturnTarget() {
            var source = new SourceClass();
            var subject = ModelMapper.Make<TargetClass>().From(source);
            subject.Should().BeOfType<TargetClass>();
            subject.Should().NotBeNull();
        }

        [Fact]
        public void ObjectSetter_From_WithNullSourceAndTarget_ShouldReturnTarget() {
            var target = new TargetClass();
            var subject = ModelMapper.Set(target).From(default(SourceClass));
            subject.Should().BeOfType<TargetClass>();
            subject.Should().BeSameAs(target);
        }

        [Fact]
        public void ObjectSetter_From_WithNullSourceAndNullTarget_ShouldReturnNullTarget() {
            var subject = ModelMapper.Make<TargetClass>().From(default(SourceClass));
            subject.Should().BeNull();
        }

        [Fact]
        public void ObjectSetter_From_SourcePropertyIsAssignableToTargetProperty_ShouldBeCopied() {
            var unmapped1 = 7;
            var unmapped2 = new SomeClass();
            var target = new TargetClass {
                Unmapped1 = unmapped1,
                Unmapped2 = unmapped2,
            };
            var source = new SourceClass {
                Property1 = 42,
                Property2 = 3.14,
                Property3 = 2.17m,
                Property4 = DateTime.Parse("2021-02-21 22:30:00"),
                Property5 = "Some String.",
                Property6 = new SomeStruct { Property1 = 42 },
                Property7 = new SomeRecord { Property1 = 42 },
                Property8 = new SomeClass { Property1 = 42 },
                Property9 = new SomeClass { Property1 = 42 },
                Property10 = new[] { 101, 201 },
                Property11 = new List<string> { "Alice", "Bob" },
                Property12 = new List<SomeStruct> { new() { Property1 = 100 }, new() { Property1 = 200 } },
                Property13 = new List<SomeRecord> { new() { Property1 = 100 }, new() { Property1 = 200 } },
                Property14 = new List<SomeClass> { new() { Property1 = 100 }, new() { Property1 = 200 } },
                Property15 = new[] { 100, 200 },
                Property16 = new List<int> { 100, 200 },
            };
            var subject = ModelMapper.Set(target).From(source);
            subject.Should().BeOfType<TargetClass>();
            subject.Should().BeSameAs(target);
            target.Property1.Should().Be(source.Property1);
            target.Property2.Should().Be(source.Property2);
            target.Property3.Should().Be(source.Property3);
            target.Property4.Should().Be(source.Property4);
            
            target.Property5.Should().BeEquivalentTo(source.Property5);
            target.Property5.Should().NotBeSameAs(source.Property5);
            target.Property6.Should().BeEquivalentTo(source.Property6);
            target.Property6.Should().NotBeSameAs(source.Property6);

            target.Property7.Should().BeEquivalentTo(source.Property7);
            target.Property7.Should().NotBeSameAs(source.Property7);
            target.Property8.Should().BeEquivalentTo(source.Property8);
            target.Property8.Should().NotBeSameAs(source.Property8);
            target.Property9.Should().BeEquivalentTo(source.Property9);
            target.Property9.Should().NotBeSameAs(source.Property9);

            target.Property10.Should().BeEquivalentTo(source.Property10);
            target.Property10.Should().NotBeSameAs(source.Property10);
            target.Property11.Should().BeEquivalentTo(source.Property11);
            target.Property11.Should().NotBeSameAs(source.Property11);
            target.Property12.Should().BeEquivalentTo(source.Property12);
            target.Property12.Should().NotBeSameAs(source.Property12);
            target.Property13.Should().BeEquivalentTo(source.Property13);
            target.Property13.Should().NotBeSameAs(source.Property13);
            target.Property14.Should().BeEquivalentTo(source.Property14);
            target.Property14.Should().NotBeSameAs(source.Property14);
            target.Property15.Should().BeEquivalentTo(source.Property15);
            target.Property15.Should().NotBeSameAs(source.Property15);
            target.Property16.Should().BeEquivalentTo(source.Property16);
            target.Property16.Should().NotBeSameAs(source.Property16);
            target.Unmapped1.Should().Be(unmapped1);
        }

        [Fact]
        public void ObjectSetter_From_TargetPropertyWithSameNameDifferentType_ShouldThrow() {
            var target = new InvalidTargetClass {
                Property1 = "1",
            };
            var source = new SourceClass {
                Property1 = 42,
                Property2 = 3.14,
                Property3 = 2.17m,
                Property5 = "Some String.",
                Property4 = DateTime.Parse("2021-02-21 22:30:00"),
            };
            Action action = () => ModelMapper.Set(target).From(source);
            action.Should().Throw<InvalidCastException>()
                .WithMessage("Mapping error: property 'Property1' of type 'System.Int32' of 'Model.Mapper.Tests.Model.SourceClass' can't be assigned to property 'Property1' of type 'System.String' of 'Model.Mapper.Tests.Model.InvalidTargetClass'.");
            target.Property1.Should().Be(target.Property1);
        }
    }
}
