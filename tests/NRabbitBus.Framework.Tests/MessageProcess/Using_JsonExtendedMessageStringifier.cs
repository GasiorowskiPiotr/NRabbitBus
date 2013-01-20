using FluentAssertions;
using NLog;
using NRabbitBus.Framework.MessageProcess;
using NUnit.Framework;

namespace NRabbitBus.Framework.Tests.MessageProcess
{
    [TestFixture]
    public class Using_JsonExtendedMessageStringifier
    {
        [Test]
        public void It_should_call_all_methods_while_processing()
        {
            var aaa = new TestJsonExtendedMessageStringifier(null).Stringify(new MyTestMessage() { Set = "aaaa" });

            var bbb = new TestJsonExtendedMessageStringifier(null).Destringify(aaa);

            TestJsonExtendedMessageStringifier.OnAfterDestringifyCalled.Should().BeTrue();
            TestJsonExtendedMessageStringifier.OnAfterStringifyCalled.Should().BeTrue();
            TestJsonExtendedMessageStringifier.OnBeforeDestringifyCalled.Should().BeTrue();
            TestJsonExtendedMessageStringifier.OnBeforeStringifyCalled.Should().BeTrue();
        }
    }

    public class TestJsonExtendedMessageStringifier : JsonExtendedMessageStringifier
    {
        public TestJsonExtendedMessageStringifier(Logger logger) : base(logger)
        {
        }

        public override void OnBeforeStringify(object obj)
        {
            OnBeforeStringifyCalled = true;
        }

        public static bool OnBeforeStringifyCalled { get; set; }

        public override void OnAfterStringify(ref string objStringForm)
        {
            OnAfterStringifyCalled = true;
        }

        public static bool OnAfterStringifyCalled { get; set; }

        public override void OnBeforeDestringify(ref string objStringForm)
        {
            OnBeforeDestringifyCalled = true;
        }

        public static bool OnBeforeDestringifyCalled { get; set; }

        public override void OnAfterDestringify(object obj)
        {
            OnAfterDestringifyCalled = true;
        }

        public static bool OnAfterDestringifyCalled { get; set; }
    }

    [TestFixture]
    public class Using_Utf8ExtendedMessageSerializer
    {
        [Test]
        public void It_should_call_all_methods_while_processing()
        {
            var aaa = new TestUtf8ExtenededMessageSerializer(null).Serialize("abc");

            var bbb = new TestUtf8ExtenededMessageSerializer(null).Deserialize(aaa);

            TestUtf8ExtenededMessageSerializer.OnBeforeSerializeCalled.Should().BeTrue();
            TestUtf8ExtenededMessageSerializer.OnAfterDeserializeCalled.Should().BeTrue();
        }
    }

    public class TestUtf8ExtenededMessageSerializer : Utf8ExtendedMessageSerializer
    {
        public TestUtf8ExtenededMessageSerializer(Logger logger) : base(logger)
        {
        }

        public override void OnBeforeSerialize(ref string objStringForm)
        {
            OnBeforeSerializeCalled = true;
        }

        public static bool OnBeforeSerializeCalled { get; set; }

        public override void OnAfterDeserialize(ref string objStringForm)
        {
            OnAfterDeserializeCalled = true;
        }

        public static bool OnAfterDeserializeCalled { get; set; }
    }
}
