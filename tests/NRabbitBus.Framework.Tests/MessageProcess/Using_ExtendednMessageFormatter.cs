using System.Text;
using FluentAssertions;
using NRabbitBus.Framework.MessageProcess;
using NUnit.Framework;

namespace NRabbitBus.Framework.Tests.MessageProcess
{
    [TestFixture]
    public class Using_ExtendednMessageFormatter
    {
        [Test]
        public void Formats_and_Deformats()
        {
            var f = new TestExtenededMessageFormatter(new Utf8MessageSerializer(null), new JsonMessageStringifier(null));

            var a = f.Deformat(f.Format(new MyTestMessage() {Set = "DDD"}));

            ((MyTestMessage) a).Set.Should().Be("DDD");
        }

        [Test]
        public void Can_alter_Message_before_formatting()
        {
            var f = new TestExtenededMessageFormatter(new Utf8MessageSerializer(null), new JsonMessageStringifier(null));

            var bytes = f.Format(new MyTestMessage() {Set = "DDD"});

            var obj = new JsonMessageStringifier(null).Destringify(Encoding.UTF8.GetString(bytes));

            ((MyTestMessage) obj).Altered.Should().Be("AAA");
        }

        [Test]
        public void Can_alter_Message_after_deformatting()
        {
            var f = new TestExtenededMessageFormatter(new Utf8MessageSerializer(null), new JsonMessageStringifier(null));

            var a = f.Deformat(f.Format(new MyTestMessage() { Set = "DDD" }));

            ((MyTestMessage)a).Altered.Should().Be("AAA");
        }
    }

    public class TestExtenededMessageFormatter : ExtendedMessageFormatter
    {
        public TestExtenededMessageFormatter(IMessageSerializer messageSerializer, IMessageStringifier messageStringifier)
            : base(messageSerializer, messageStringifier, null)
        {
        }

        public override void OnBeforeFormat(object obj)
        {
            var myTestMessage = obj as MyTestMessage;
            if (myTestMessage != null)
                myTestMessage.Altered = "AAA";
        }

        public override void OnAfterDeformat(object obj)
        {
            var myTestMessage = obj as MyTestMessage;
            if (myTestMessage != null)
                myTestMessage.Altered = "AAA";
        }
    }

    public class MyTestMessage
    {
        public string Altered { get; set; }
        public string Set { get; set; }
    }
}
