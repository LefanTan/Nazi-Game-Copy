using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using ModestTree;
using Assert=ModestTree.Assert;
using Zenject.Tests.Bindings.FromScriptableObjectResource;

namespace Zenject.Tests.Bindings
{
    [TestFixture]
    public class TestFromScriptableObjectResource : ZenjectIntegrationTestFixture
    {
        const string PathPrefix = "TestFromScriptableObjectResource/";

        [Test]
        [ExpectedException]
        public void TestTransientError()
        {
            // Validation should detect that it doesn't exist
            Container.Bind<Foo>().FromScriptableObjectResource(PathPrefix + "asdfasdfas").AsTransient().NonLazy();

            Initialize();
        }

        [Test]
        public void TestTransient()
        {
            Foo.InstanceCount = 0;
            Container.Bind<Foo>().FromScriptableObjectResource(PathPrefix + "Foo").AsTransient();

            Initialize();

            var foo = Container.Resolve<Foo>();
            Assert.That(foo.WasInjected);

            Assert.IsEqual(Foo.InstanceCount, 1);

            var foo2 = Container.Resolve<Foo>();
            Assert.IsNotEqual(foo, foo2);
            Assert.IsEqual(Foo.InstanceCount, 2);
        }

        [Test]
        public void TestSingle()
        {
            Foo.InstanceCount = 0;

            Container.Bind<IFoo>().To<Foo>().FromScriptableObjectResource(PathPrefix + "Foo").AsSingle();
            Container.Bind<Foo>().FromScriptableObjectResource(PathPrefix + "Foo").AsSingle();

            Initialize();

            var ifoo = Container.Resolve<IFoo>();
            Assert.IsEqual(Foo.InstanceCount, 1);
        }

        [Test]
        public void TestAbstractBinding()
        {
            Foo.InstanceCount = 0;

            Container.Bind<IFoo>().To<Foo>()
                .FromScriptableObjectResource(PathPrefix + "Foo").AsSingle().NonLazy();

            Initialize();

            var foo = Container.Resolve<IFoo>();
            Assert.IsEqual(Foo.InstanceCount, 1);
        }

        [Test]
        [ExpectedException]
        public void TestWithArgumentsFail()
        {
            Container.Bind<Bob>()
                .FromScriptableObjectResource(PathPrefix + "Bob").AsCached().NonLazy();

            Initialize();
        }

        [Test]
        public void TestWithArguments()
        {
            Container.Bind<Bob>()
                .FromScriptableObjectResource(PathPrefix + "Bob").AsCached()
                .WithArguments("test1").NonLazy();

            Initialize();

            Assert.IsEqual(Container.Resolve<Bob>().Arg, "test1");
        }
    }
}
