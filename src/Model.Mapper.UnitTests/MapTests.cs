using System;
using System.Collections.Generic;
using FluentAssertions;
using Model.Mapper.Tests.Model;
using Xunit;

namespace Model.Mapper.Tests {
    public class MapTests {
        [Fact]
        public void Map_From_ReturnsMapper() {
            var source = new SomeClass();
            var subject = Map.From(source);
            subject.Should().BeAssignableTo<IMapper>();
        }

        [Fact]
        public void Map_From_WithNullSource_ShouldThrow() {
            Action action = () => Map.From(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Map_FromSource_ToType_AsGenerics_ShouldReturnNewTarget() {
            var source = new EmptySource();
            var subject = Map.From(source).To<EmptyTarget>();
            subject.Should().BeOfType<EmptyTarget>();
            subject.Should().NotBeNull();
        }

        [Fact]
        public void Map_FromSource_ToType_AsParameter_ShouldReturnNewTarget() {
            var source = new EmptySource();
            var subject = Map.From(source).To(typeof(EmptyTarget));
            subject.Should().BeOfType<EmptyTarget>();
            subject.Should().NotBeNull();
        }

        [Fact]
        public void Map_ForReferenceType_ShouldReturnSameTargetInstance() {
            var target = new EmptyTarget();
            var source = new EmptySource();
            Map.From(source).To(target);
            target.Should().BeSameAs(target);
        }

        [Fact]
        public void Map_ToInterface_ShouldThrow() {
            var source = new EmptySource();
            Action action = () => Map.From(source).To<ISomeClass>();
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Map_ToAssignableType_ShouldReturnSource() {
            var source = new SomeClass();
            var target = Map.From(source).To<ISomeClass>();
            target.Should().BeEquivalentTo(source);
        }

        [Fact]
        public void Map_ToDictionary_ShouldReturnNewDictionary() {
            var source = new Dictionary<string, EmptySource> { ["A"] = new(), ["B"] = new() };
            var target = Map.From(source).To<Dictionary<string, EmptyTarget>>();
            target.Count.Should().Be(source.Count);
        }

        [Fact]
        public void Map_ToList_ShouldReturnNewList() {
            var source = new List<EmptySource> { new(), new() };
            var target = Map.From(source).To<List<EmptyTarget>>();
            target.Count.Should().Be(source.Count);
        }

        [Fact]
        public void Map_ToArray_ShouldReturnNewArray() {
            var source = new List<EmptySource> { new(), new() };
            var target = Map.From(source).To<EmptyTarget[]>();
            target.Length.Should().Be(source.Count);
        }

        [Fact]
        public void Map_ForComplexTypes_WithValueTypeProperties_ShouldCopyProperties() {
            var originalString = "Original String.";
            var originalStruct = new SomeStruct();
            var target = new TargetWithValueTypes {
                String = originalString,
                Struct = originalStruct,
            };
            var source = new SourceWithValueTypes {
                Integer = 42,
                Double = 3.14,
                Decimal = 2.17m,
                DateTime = DateTime.Parse("2021-02-21 22:30:00"),
                String = "Some String.",
                Struct = new SomeStruct { Property1 = 42 },
            };
            Map.From(source).To(target);
            target.Should().BeSameAs(target);
            target.Integer.Should().Be(source.Integer);
            target.Double.Should().Be(source.Double);
            target.Decimal.Should().Be(source.Decimal);
            target.DateTime.Should().Be(source.DateTime);
            
            target.String.Should().BeEquivalentTo(source.String);
            target.String.Should().NotBeSameAs(originalString);
            target.Struct.Should().BeEquivalentTo(source.Struct);
            target.Struct.Should().NotBeSameAs(originalStruct);
        }

        [Fact]
        public void Map_ForComplexTypes_WithReferenceTypeProperties_ShouldUpdateProperties() {
            var originalRecord = new SomeRecord {Property1 = 42};
            var originalClass = new SomeClass {Property1 = 42};
            var originalInterface = new SomeClass {Property1 = 42};
            var target = new TargetWithReferenceTypes {
                Record = originalRecord,
                Class = originalClass,
                Interface = originalInterface,
                NullClass = null,
                NullInterface = null,
                NullableClass = new SomeClass { Property1 = 42 },
            };
            var source = new SourceWithReferenceTypes {
                Record = new SomeRecord { Property1 = 42 },
                Class = new SomeClass { Property1 = 42 },
                Interface = new SomeClass { Property1 = 42 },
                NullClass = new SomeClass { Property1 = 42 },
                NullInterface = new SomeClass { Property1 = 42 },
                NullableClass = null,
            };
            Map.From(source).To(target);
            target.Should().BeSameAs(target);
            target.Record.Should().BeEquivalentTo(source.Record);
            target.Record.Should().NotBeSameAs(originalRecord);
            target.Class.Should().BeEquivalentTo(source.Class);
            target.Class.Should().BeSameAs(originalClass);
            target.Interface.Should().BeEquivalentTo(source.Interface);
            target.Interface.Should().BeSameAs(originalInterface);
            target.NullClass.Should().NotBeNull();
            target.NullInterface.Should().NotBeNull();
            target.NullableClass.Should().BeNull();
        }

        [Fact]
        public void Map_To_ForTargetWithCollections_ShouldBeCopied() {
            var originalArray = Array.Empty<int>();
            var originalList = new List<int>();
            var originalHashSet = new HashSet<int>();
            var originalDictionary = new Dictionary<int, string>();
            var originalEnumerableInterface = Array.Empty<int>();
            var originalCollectionInterface = new List<int>();
            var originalListInterface = new List<int>();
            var originalSetInterface = new HashSet<int>();
            var originalDictionaryInterface = new Dictionary<int, string>();
            var target = new TargetWithCollections {
                Array = originalArray,

                List = originalList,
                HashSet = originalHashSet,
                Dictionary = originalDictionary,

                EnumerableInterface = originalEnumerableInterface,
                CollectionInterface = originalCollectionInterface,
                ListInterface = originalListInterface,
                SetInterface = originalSetInterface,
                DictionaryInterface = originalDictionaryInterface,

                Classes = null,
                Index = null,
            };
            var source = new SourceWithCollections {
                Array = new[] { 100, 200 },

                List = new List<int> { 100, 200 },
                HashSet = new HashSet<int> { 100, 200 },
                Dictionary = new Dictionary<int, string> { [100] = "A", [200] = "B" },

                EnumerableInterface = new List<int> { 101, 201 },
                CollectionInterface = new[] { 100, 200 },
                ListInterface = new List<int> { 101, 201 },
                SetInterface = new HashSet<int> { 100, 200 },
                DictionaryInterface = new Dictionary<int, string> { [100] = "A", [200] = "B" },

                Classes = new List<SomeClass> { new() { Property1 = 100 }, new() { Property1 = 200 } },
                Index = new Dictionary<string, SomeClass> { ["A"] = new() { Property1 = 100 }, ["B"] = new() { Property1 = 200 } },
            };
            Map.From(source).To(target);
            target.Should().BeSameAs(target);
            target.Array.Should().BeEquivalentTo(source.Array);
            target.Array.Should().NotBeSameAs(originalArray);

            target.List.Should().BeEquivalentTo(source.List);
            target.List.Should().BeSameAs(originalList);
            target.HashSet.Should().BeEquivalentTo(source.HashSet);
            target.HashSet.Should().BeSameAs(originalHashSet);
            target.Dictionary.Should().BeEquivalentTo(source.Dictionary);
            target.Dictionary.Should().BeSameAs(originalDictionary);

            target.EnumerableInterface.Should().BeEquivalentTo(source.EnumerableInterface);
            target.EnumerableInterface.Should().NotBeSameAs(originalEnumerableInterface);
            target.CollectionInterface.Should().BeEquivalentTo(source.CollectionInterface);
            target.CollectionInterface.Should().BeSameAs(originalCollectionInterface);
            target.ListInterface.Should().BeEquivalentTo(source.ListInterface);
            target.ListInterface.Should().BeSameAs(originalListInterface);
            target.SetInterface.Should().BeSameAs(originalSetInterface);
            target.SetInterface.Should().BeSameAs(originalSetInterface);
            target.DictionaryInterface.Should().BeEquivalentTo(source.DictionaryInterface);
            target.DictionaryInterface.Should().BeSameAs(originalDictionaryInterface);

            target.Classes.Should().BeEquivalentTo(source.Classes);
            target.Index.Should().BeEquivalentTo(source.Index);
        }

        [Fact]
        public void Map_To_ForTargetWithUnmappedProperties_ShouldBeCopied() {
            var unmapped1 = 7;
            var unmapped2 = new SomeClass();
            var target = new TargetWithUnmappedProperties {
                Unmapped1 = unmapped1,
                Unmapped2 = unmapped2,
            };
            var source = new EmptySource();
            Map.From(source).To(target);
            target.Should().BeSameAs(target);
            target.Unmapped1.Should().Be(unmapped1);
            target.Unmapped2.Should().BeSameAs(unmapped2);
        }

        [Fact]
        public void Map_To_TargetWithInvalidMapping_ShouldThrow() {
            var target = new TargetWithInvalidMapping {
                Integer = "1",
            };
            var source = new SourceWithValueTypes {
                Integer = 42,
                Double = 3.14,
                Decimal = 2.17m,
                String = "Some String.",
                DateTime = DateTime.Parse("2021-02-21 22:30:00"),
            };
            Action action = () => Map.From(source).To(target);
            action.Should().Throw<InvalidCastException>()
                .WithMessage("Mapping error: property 'Integer' of type 'System.Int32' of 'Model.Mapper.Tests.Model.SourceWithValueTypes' can't be mapped to property 'Integer' of type 'System.String' of 'Model.Mapper.Tests.Model.TargetWithInvalidMapping'.");
            target.Integer.Should().Be(target.Integer);
        }
    }
}
