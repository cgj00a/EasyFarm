using System;
using EasyFarm.Tests.TestTypes;
using Xunit;

namespace EasyFarm.Tests.Builders
{
    public class BuilderTest
    {
        [Fact]
        public void BuildCreateReturnsCorrectType()
        {
            // Fixture setup
            var sut = new Builder<ConcreteType>(new ConcreteType());
            // Exercise system
            var result = sut.Build();
            // Verify outcome	
            Assert.IsAssignableFrom<ConcreteType>(result);
            // Teardown
        }

        [Fact]
        public void BuildWithPropertyReturnsCorrectResult()
        {
            // Fixture setup            
            var sut = new Builder<ConcreteType>(new ConcreteType());
            // Exercise system
            var result = sut.With(x => x.PropertyValue, 1).Build().PropertyValue;
            // Verify outcome	
            Assert.Equal(1, result);
            // Teardown
        }

        [Fact]
        public void BuildWithFieldReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new Builder<ConcreteType>(new ConcreteType());
            // Exercise system
            var result = sut.With(x => x.FieldValue, 1).Build().FieldValue;
            // Verify outcome	
            Assert.Equal(1, result);
            // Teardown
        }

        [Fact]
        public void BuildWithMethodThrowsArgumentException()
        {
            // Fixture setup
            var sut = new Builder<ConcreteType>(new ConcreteType());
            // Exercise system
            Assert.Throws<ArgumentException>(() => sut.With(x => x.MethodCall(), 1));
            // Verify outcome	
            // Teardown
        }

        [Fact]
        public void BuildWithPrivateSetterPropertyThrowsArgumentException()
        {
            // Fixture setup            
            var sut = new Builder<ConcreteType>(new ConcreteType());
            // Exercise system
            Assert.Throws<ArgumentException>(() => sut.With(x => x.PrivateSetterProperty, 1));
            // Verify outcome	
            // Teardown
        }
    }
}