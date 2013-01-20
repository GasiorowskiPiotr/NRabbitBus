using FluentAssertions;
using NUnit.Framework;

namespace EvilDuck.Framework.Tests.Configuration
{
    [TestFixture]
    public class Having_configuration
    {
        private SampleSection sampleSection;

        [SetUp]
        public void Setup()
        {
            sampleSection = Framework.Configuration.Configuration<SampleSection>.Current;
        }

        [Test]
        public void Section_should_have_proper_defined_value()
        {
            sampleSection.Data.Should().Be("Data");
        }

        [Test]
        public void Section_should_have_propert_default_value()
        {
            sampleSection.DefaultVal.Should().Be(666);
        }

        [Test]
        public void Section_should_have_nonNull_nested_element()
        {
            sampleSection.Nested.Should().NotBeNull();
        }

        [Test]
        public void Section_should_have_propert_value_in_nested_element()
        {
            sampleSection.Nested.Float.Should().Be(123.456f);
        }


    }
}
