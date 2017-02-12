using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using ModestTree;
using Assert=ModestTree.Assert;
using Zenject.Tests.Bindings.FromSubContainerPrefabResource;

namespace Zenject.Tests.Bindings
{
    [TestFixture]
    public class TestFromSubContainerPrefabResource : ZenjectIntegrationTestFixture
    {
        const string PathPrefix = "TestFromSubContainerPrefabResource/";
        const string FooResourcePath = PathPrefix + "FooSubContainer";

        [Test]
        [ExpectedException]
        public void TestTransientError()
        {
            // Validation should detect that it doesn't exist
            Container.Bind<Foo>().FromSubContainerResolve().ByPrefabResource(PathPrefix + "asdfasdfas").AsTransient().NonLazy();

            Initialize();
        }

        [Test]
        public void TestSelfSingle()
        {
            Container.Bind<Foo>().FromSubContainerResolve().ByPrefabResource(FooResourcePath).AsSingle();

            Container.BindRootResolve<Foo>();
            Container.BindRootResolve<Foo>();
            Container.BindRootResolve<Foo>();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
        }

        [Test]
        public void TestSelfTransient()
        {
            Container.Bind<Foo>().FromSubContainerResolve().ByPrefabResource(FooResourcePath).AsTransient();

            Container.BindRootResolve<Foo>();
            Container.BindRootResolve<Foo>();
            Container.BindRootResolve<Foo>();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 3);
            FixtureUtil.AssertComponentCount<Foo>(Container, 3);
        }

        [Test]
        public void TestSelfCached()
        {
            Container.Bind<Foo>().FromSubContainerResolve().ByPrefabResource(FooResourcePath).AsCached();

            Container.BindRootResolve<Foo>();
            Container.BindRootResolve<Foo>();
            Container.BindRootResolve<Foo>();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
        }

        [Test]
        public void TestSelfSingleMultipleContracts()
        {
            Container.Bind<Foo>().FromSubContainerResolve().ByPrefabResource(FooResourcePath).AsSingle().NonLazy();
            Container.Bind<Bar>().FromSubContainerResolve().ByPrefabResource(FooResourcePath).AsSingle().NonLazy();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertComponentCount<Bar>(Container, 1);
        }

        [Test]
        public void TestSelfCachedMultipleContracts()
        {
            Container.Bind(typeof(Foo), typeof(Bar)).FromSubContainerResolve().ByPrefabResource(FooResourcePath).AsCached().NonLazy();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertComponentCount<Bar>(Container, 1);
        }

        [Test]
        public void TestSelfTransientMultipleContracts()
        {
            Container.Bind(typeof(Foo), typeof(Bar)).FromSubContainerResolve().ByPrefabResource(FooResourcePath).AsTransient().NonLazy();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 2);
            FixtureUtil.AssertComponentCount<Foo>(Container, 2);
            FixtureUtil.AssertComponentCount<Bar>(Container, 2);
        }

        [Test]
        public void TestConcreteSingle()
        {
            Container.Bind<IFoo>().To<Foo>().FromSubContainerResolve().ByPrefabResource(FooResourcePath).AsSingle();

            Container.BindRootResolve<IFoo>();
            Container.BindRootResolve<IFoo>();
            Container.BindRootResolve<IFoo>();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
        }

        [Test]
        public void TestConcreteTransient()
        {
            Container.Bind<IFoo>().To<Foo>().FromSubContainerResolve().ByPrefabResource(FooResourcePath).AsTransient();

            Container.BindRootResolve<IFoo>();
            Container.BindRootResolve<IFoo>();
            Container.BindRootResolve<IFoo>();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 3);
            FixtureUtil.AssertComponentCount<Foo>(Container, 3);
        }

        [Test]
        public void TestConcreteCached()
        {
            Container.Bind<IFoo>().To<Foo>().FromSubContainerResolve().ByPrefabResource(FooResourcePath).AsCached();

            Container.BindRootResolve<IFoo>();
            Container.BindRootResolve<IFoo>();
            Container.BindRootResolve<IFoo>();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
        }

        [Test]
        public void TestConcreteSingleMultipleContracts()
        {
            Container.Bind<IFoo>().To<Foo>().FromSubContainerResolve().ByPrefabResource(FooResourcePath).AsSingle();
            Container.Bind<Bar>().FromSubContainerResolve().ByPrefabResource(FooResourcePath).AsSingle();

            Container.BindRootResolve<IFoo>();
            Container.BindRootResolve<IFoo>();
            Container.BindRootResolve<IFoo>();
            Container.BindRootResolve<Bar>();
            Container.BindRootResolve<Bar>();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertComponentCount<Bar>(Container, 1);
        }

        [Test]
        public void TestConcreteCachedMultipleContracts()
        {
            Container.Bind(typeof(Foo), typeof(IFoo)).To<Foo>().FromSubContainerResolve().ByPrefabResource(FooResourcePath).AsCached();

            Container.BindRootResolve<IFoo>();
            Container.BindRootResolve<IFoo>();
            Container.BindRootResolve<IFoo>();
            Container.BindRootResolve<Foo>();
            Container.BindRootResolve<Foo>();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
        }

        [Test]
        [ExpectedException]
        public void TestSelfIdentifiersFails()
        {
            Container.Bind<Gorp>().FromSubContainerResolve().ByPrefabResource(FooResourcePath).AsSingle().NonLazy();

            Initialize();
        }

        [Test]
        public void TestSelfIdentifiers()
        {
            Container.Bind<Gorp>().FromSubContainerResolve("gorp").ByPrefabResource(FooResourcePath).AsSingle();

            Container.BindRootResolve<Gorp>();
            Container.BindRootResolve<Gorp>();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 1);
        }
    }
}
