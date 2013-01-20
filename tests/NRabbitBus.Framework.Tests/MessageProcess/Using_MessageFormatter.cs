using FluentAssertions;
using NRabbitBus.Framework.MessageProcess;
using NUnit.Framework;

namespace NRabbitBus.Framework.Tests.MessageProcess
{
    [TestFixture]
    public class Using_MessageFormatter
    {
        [Test]
        public void Should_be_reversive()
        {
            var messageFormatter = new MessageFormatter(new Utf8MessageSerializer(null), new JsonMessageStringifier(null), null);

            var x = messageFormatter.Deformat(messageFormatter.Format(new MyTestMessage()
                                                                  {
                                                                      Set = "DDDDD"
                                                                  }));

            ((MyTestMessage) x).Set.Should().Be("DDDDD");
        }
    }
}
