using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using ModestTree;
using Assert=ModestTree.Assert;
using Zenject.Tests.Bindings.FromSubContainerPrefab;

#pragma warning disable 649

namespace Zenject.Tests.Bindings
{
    [TestFixture]
    public class TestFromSubContainerPrefab : ZenjectIntegrationTestFixture
    {
        GameObject FooPrefab
        {
            get
            {
                return FixtureUtil.GetPrefab("TestFromSubContainerPrefab/Foo");
            }
        }

        GameObject FooPrefab2
        {
            get
            {
                return FixtureUtil.GetPrefab("TestFromSubContainerPrefab/Foo2");
            }
        }

        [Test]
        public void TestSelfSingle()
        {
            Container.Bind<Foo>().FromSubContainerResolve()
                .ByPrefab(FooPrefab).AsSingle();

            Container.BindRootResolve<Foo>();
            Container.BindRootResolve<Foo>();
            Container.BindRootResolve<Foo>();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
        }

        [Test]
        [ValidateOnly]
        public void TestSelfSingleValidate()
        {
            Container.Bind<Foo>().FromSubContainerResolve()
                .ByPrefab(FooPrefab).AsSingle().NonLazy();

            Initialize();
        }

        [Test]
        [ValidateOnly]
        [ExpectedException]
        public void TestSelfSingleValidateFails()
        {
            Container.Bind<Foo>().FromSubContainerResolve()
                .ByPrefab(FooPrefab2).AsSingle().NonLazy();

            Initialize();
        }

        [Test]
        public void TestSelfTransient()
        {
            Container.Bind<Foo>().FromSubContainerResolve().ByPrefab(FooPrefab).AsTransient();

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
            Container.Bind<Foo>().FromSubContainerResolve().ByPrefab(FooPrefab).AsCached();

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
            Container.Bind<Foo>().FromSubContainerResolve().ByPrefab(FooPrefab).AsSingle();
            Container.Bind<Bar>().FromSubContainerResolve().ByPrefab(FooPrefab).AsSingle();

            Container.BindRootResolve<Foo>();
            Container.BindRootResolve<Bar>();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertComponentCount<Bar>(Container, 1);
        }

        [Test]
        public void TestSelfCachedMultipleContracts()
        {
            Container.Bind(typeof(Foo), typeof(Bar)).FromSubContainerResolve().ByPrefab(FooPrefab).AsCached();

            Container.BindRootResolve<Foo>();
            Container.BindRootResolve<Bar>();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertComponentCount<Bar>(Container, 1);
        }

        [Test]
        public void TestSelfTransientMultipleContracts()
        {
            Container.Bind(typeof(Foo), typeof(Bar)).FromSubContainerResolve().ByPrefab(FooPrefab).AsTransient();

            Container.BindRootResolve<Foo>();
            Container.BindRootResolve<Bar>();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 2);
            FixtureUtil.AssertComponentCount<Foo>(Container, 2);
            FixtureUtil.AssertComponentCount<Bar>(Container, 2);
        }

        [Test]
        public void TestConcreteSingle()
        {
            Container.Bind<IFoo>().To<Foo>().FromSubContainerResolve().ByPrefab(FooPrefab).AsSingle();

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
            Container.Bind<IFoo>().To<Foo>().FromSubContainerResolve().ByPrefab(FooPrefab).AsTransient();

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
            Container.Bind<IFoo>().To<Foo>().FromSubContainerResolve().ByPrefab(FooPrefab).AsCached();

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
            Container.Bind<IFoo>().To<Foo>().FromSubContainerResolve().ByPrefab(FooPrefab).AsSingle();
            Container.Bind<Bar>().FromSubContainerResolve().ByPrefab(FooPrefab).AsSingle();

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
            Container.Bind(typeof(Foo), typeof(IFoo)).To<Foo>().FromSubContainerResolve().ByPrefab(FooPrefab).AsCached();

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
            Container.Bind<Gorp>().FromSubContainerResolve().ByPrefab(FooPrefab).AsSingle();

            Container.BindRootResolve<Gorp>();

            Initialize();
        }

        [Test]
        public void TestSelfIdentifiers()
        {
            Container.Bind<Gorp>().FromSubContainerResolve("gorp").ByPrefab(FooPrefab).AsSingle();

            Container.BindRootResolve<Gorp>();
            Container.BindRootResolve<Gorp>();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 1);
        }
    }
}
