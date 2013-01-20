using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NRabbitBus.Framework.MessageProcess;
using NUnit.Framework;

namespace NRabbitBus.Framework.Tests.MessageProcess
{
    [TestFixture]
    public class JsonMessageStringifierTest
    {
        public class MyType
        {
            public string MyString { get; set; }
            public int MyInt { get; set; }
            public float MyFloat { get; set; }

            public MySubType SubType { get; set; }
            public IEnumerable<MySubType> SubTypes { get; set; } 
        }

        public class MySubType
        {
            public string A { get; set; }
            public int B { get; set; }
        }

        [Test]
        public void Stringifying_and_destringifying_should_be_reversive()
        {
            var myType = new MyType()
                             {
                                 MyFloat = 123.456f,
                                 MyInt = 543,
                                 MyString = "HelloWorld",
                                 SubType = new MySubType()
                                               {
                                                   A = "Piotr",
                                                   B = 5
                                               },
                                 SubTypes = new List<MySubType>()
                                                {
                                                    new MySubType()
                                                        {
                                                            A = "Ewelinka",
                                                            B = 6
                                                        },
                                                    new MySubType()
                                                        {
                                                            A = "Micha³",
                                                            B = 7
                                                        }
                                                }
                             };

            var stringifier = new JsonMessageStringifier(null);

            var res = stringifier.Destringify(stringifier.Stringify(myType));

            res.Should().BeOfType<MyType>();

            var resT = (MyType) res;

            resT.MyFloat.Should().Be(myType.MyFloat);
            resT.MyInt.Should().Be(myType.MyInt);
            resT.MyString.Should().Be(myType.MyString);

            resT.SubType.A.Should().Be(myType.SubType.A);
            resT.SubType.B.Should().Be(myType.SubType.B);

            resT.SubTypes.Count().Should().Be(myType.SubTypes.Count());
        }

        [Test]
        public void It_should_be_able_to_stringify_Null()
        {
            var stringifier = new JsonMessageStringifier(null);
            var result = stringifier.Stringify(null);

            result.Should().Be("null");
        }

        [Test]
        public void It_should_be_able_to_destringify_Null_as_string()
        {
            var stringifier = new JsonMessageStringifier(null);
            var result1 = stringifier.Destringify("null");

            result1.Should().BeNull();

            
        }

        [Test]
        public void Destringifying_Null_value_should_throw()
        {
            var stringifier = new JsonMessageStringifier(null);

            Assert.Throws<ArgumentNullException>(() => stringifier.Destringify(null));
        }
    }
}