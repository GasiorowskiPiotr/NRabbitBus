using System;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;

namespace EvilDuck.Framework.Tests.Cache
{
    [TestFixture]
    public class Having_Cache
    {
        protected static readonly SampleCache TestCache = new SampleCache();

        [TestFixture]
        public class When_adding_element_to_cache
        {
            [SetUp]
            public void Setup()
            {
                TestCache.Add(1, "aaa");
            }

            [Test]
            public void We_should_be_able_to_retrieve_added_data()
            {
                var data = TestCache.Get(1);

                data.Should().NotBeNullOrEmpty();
            }

            [Test]
            public void We_should_be_able_to_retrived_exactly_the_same_data()
            {
                var data = TestCache.Get(1);

                data.Should().Be("aaa");
            }

            [TearDown]
            public void Teardown()
            {
                TestCache.Invalidate();
            }
        }

        [TestFixture]
        public class When_missing_element_in_cache_and_trying_to_get_it
        {
            private string _value;

            [SetUp]
            public void Setup()
            {
                _value = TestCache.Get(3);
            }

            [Test]
            public void It_should_return_proper_value_from_OnMissed()
            {
                _value.Should().NotBeNullOrEmpty();
                _value.Should().Be("OnMiss:3");
            }

            [Test]
            public void It_should_have_this_value_stored_in_cache_for_later_use()
            {
                TestCache.Contains(3).Should().BeTrue();
            }

            [TearDown]
            public void Teardown()
            {
                TestCache.Invalidate();
            }
        }

        [TestFixture]
        public class When_ItemLifeTime_is_exceeded
        {
            [SetUp]
            public void Setup()
            {
                TestCache.Add(1, "abc");

                Thread.Sleep(TimeSpan.FromSeconds(7));
            }

            [Test]
            public void Calling_RemoveOutdated_should_remove_outdated_data()
            {
                TestCache.RemoveOutdated();

                TestCache.Contains(1).Should().BeFalse();
            }

            [Test]
            public void Calling_Get_should_hit_onMiss_once_more()
            {
                TestCache.Get(1);
                TestCache.OnMissCounter[1].Should().Be(1);
            }

            [TearDown]
            public void Teardown()
            {
                TestCache.Invalidate();
            }
        }
        
        [TestFixture]
        public class Invalidating_cache
        {
            [SetUp]
            public void Setup()
            {
                TestCache.Add(10, "a");
                TestCache.Add(11, "b");
                TestCache.Add(12, "c");
                TestCache.Add(13, "d");    

                TestCache.Invalidate();
            }

            [Test]
            public void Should_delete_all_cached_items()
            {
                TestCache.Contains(10).Should().BeFalse();
                TestCache.Contains(11).Should().BeFalse();
                TestCache.Contains(12).Should().BeFalse();
                TestCache.Contains(13).Should().BeFalse();
            }

            [Test]
            public void Should_be_able_to_perform_onMiss()
            {
                var item = TestCache.Get(10);
                item.Should().Be("OnMiss:10");
            }

            [TearDown]
            public void Teardown()
            {
                TestCache.Invalidate();
            }
        }
    }
}
