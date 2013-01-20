using FluentAssertions;
using NUnit.Framework;

namespace EvilDuck.Framework.Tests.Configuration
{
    [TestFixture]
    public class Having_NameAndValuesCollection_in_configuration
    {
        private NameAndValues _section;

        [SetUp]
        public void Setup()
        {
            _section = EvilDuck.Framework.Configuration.Configuration<NameAndValues>.Current;
        }

        [Test]
        public void It_should_get_all_elements()
        {
            _section.Data["aaa"].Value.Should().Be("bbb");
            _section.Data["ccc"].Value.Should().Be("ddd");
        }
    }
}