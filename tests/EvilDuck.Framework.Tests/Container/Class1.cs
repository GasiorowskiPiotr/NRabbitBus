using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using EvilDuck.Framework.Container;
using FluentAssertions;
using NUnit.Framework;
using Module = Autofac.Module;

namespace EvilDuck.Framework.Tests.Container
{
    [TestFixture]
    public class After_container_from_ApplicationModule_class_is_initialized
    {

        [TestFixtureSetUp]
        public void SetUp()
        {
            ContainerBootstrap.Initialize(new TestContainerModule());
        }

        [Test]
        public void It_should_have_ContainerLocator_initialized()
        {
            ComponentLocator.Current.Should().NotBeNull();
        }

        [Test]
        public void It_should_handle_singletons_properly()
        {
            var a = ComponentLocator.Current.Get<Singleton>();
            var b = ComponentLocator.Current.Get<Singleton>();
            Singleton c;

            using (var l = ComponentLocator.Current.StartChildScope())
            {
                c = l.Resolve<Singleton>();
            }

            a.Should().NotBeNull();
            b.Should().NotBeNull();
            c.Should().NotBeNull();

            a.Should().BeSameAs(b);
            b.Should().BeSameAs(c);
        }

        [Test]
        public void It_should_hanlde_transients_properly()
        {
            var a = ComponentLocator.Current.Get<Transient>();
            var b = ComponentLocator.Current.Get<Transient>();
            Transient c;

            using (var l = ComponentLocator.Current.StartChildScope())
            {
                c = l.Resolve<Transient>();
            }

            a.Should().NotBeNull();
            b.Should().NotBeNull();
            c.Should().NotBeNull();

            a.Should().NotBeSameAs(b);
            b.Should().NotBeSameAs(c);
            a.Should().NotBeSameAs(c);
        }

        [Test]
        public void It_should_consider_lifetime_scope()
        {
            PerLifetime a;
            PerLifetime b;
            PerLifetime c;
            PerLifetime d;
            PerLifetime e;
            PerLifetime f;


            using (var l = ComponentLocator.Current.StartChildScope())
            {
                a = l.Resolve<PerLifetime>();
                b = l.Resolve<PerLifetime>();
            }


            using (var l = ComponentLocator.Current.StartChildScope())
            {
                c = l.Resolve<PerLifetime>();
                d = l.Resolve<PerLifetime>();
            }

            e = ComponentLocator.Current.Get<PerLifetime>();
            f = ComponentLocator.Current.Get<PerLifetime>();

            a.Should().BeSameAs(b);
            c.Should().BeSameAs(d);
            e.Should().BeSameAs(f);

            a.Should().NotBeSameAs(c);
            a.Should().NotBeSameAs(e);

            c.Should().NotBeSameAs(e);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            ContainerBootstrap.Close();
        }

    }

    public class TestContainerModule : Module
    {
        protected override void Load(Autofac.ContainerBuilder builder)
        {
            builder
                .RegisterType<Singleton>()
                .As<Singleton>()
                .SingleInstance();

            builder
                .RegisterType<Transient>()
                .As<Transient>()
                .InstancePerDependency();

            builder
                .RegisterType<PerLifetime>()
                .As<PerLifetime>()
                .InstancePerLifetimeScope();
        }
    }

    public class Singleton
    {

    }

    public class Transient
    {

    }

    public class PerLifetime
    {

    }
}
