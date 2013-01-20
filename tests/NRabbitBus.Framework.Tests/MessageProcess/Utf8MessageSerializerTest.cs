using NRabbitBus.Framework.MessageProcess;
using NUnit.Framework;

namespace NRabbitBus.Framework.Tests.MessageProcess
{
    [TestFixture]
    public class Utf8MessageSerializerTest
    {
        [Test]
        public void Serializing_and_deserializing_should_be_reversive()
        {
            const string myString = "Piotrek jest zajebistym programist¹";

            var serializer = new Utf8MessageSerializer(null);
            var res = serializer.Deserialize(serializer.Serialize(myString));

            Assert.AreEqual(myString, res);
        }
    }
}