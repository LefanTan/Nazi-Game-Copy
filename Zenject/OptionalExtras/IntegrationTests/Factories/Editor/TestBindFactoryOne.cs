using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using ModestTree;
using Assert=ModestTree.Assert;
using Zenject.Tests.Factories.BindFactoryOne;

namespace Zenject.Tests.Factories
{
    [TestFixture]
    public class TestBindFactoryOne : ZenjectIntegrationTestFixture
    {
        GameObject FooPrefab
        {
            get
            {
                return FixtureUtil.GetPrefab("TestBindFactoryOne/Foo");
            }
        }

        GameObject FooSubContainerPrefab
        {
            get
            {
                return FixtureUtil.GetPrefab("TestBindFactoryOne/FooSubContainer");
            }
        }

        [Test]
        public void TestToGameObjectSelf()
        {
            Container.BindFactory<string, Foo, Foo.Factory>().FromNewComponentOnNewGameObject();

            AddFactoryUser<Foo, Foo.Factory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestToGameObjectConcrete()
        {
            Container.BindFactory<string, IFoo, IFooFactory>().To<Foo>().FromNewComponentOnNewGameObject();

            AddFactoryUser<IFoo, IFooFactory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestToMonoBehaviourSelf()
        {
            var gameObject = Container.CreateEmptyGameObject("foo");

            Container.BindFactory<string, Foo, Foo.Factory>().FromNewComponentOn(gameObject);

            AddFactoryUser<Foo, Foo.Factory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestToMonoBehaviourConcrete()
        {
            var gameObject = Container.CreateEmptyGameObject("foo");

            Container.BindFactory<string, IFoo, IFooFactory>().To<Foo>().FromNewComponentOn(gameObject);

            AddFactoryUser<IFoo, IFooFactory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestToPrefabSelf()
        {
            Container.BindFactory<string, Foo, Foo.Factory>().FromComponentInPrefab(FooPrefab).WithGameObjectName("asdf");

            AddFactoryUser<Foo, Foo.Factory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertNumGameObjectsWithName(Container, "asdf", 1);
        }

        [Test]
        public void TestToPrefabConcrete()
        {
            Container.BindFactory<string, IFoo, IFooFactory>().To<Foo>()
                .FromComponentInPrefab(FooPrefab).WithGameObjectName("asdf");

            AddFactoryUser<IFoo, IFooFactory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertNumGameObjectsWithName(Container, "asdf", 1);
        }

        [Test]
        public void TestToPrefabResourceSelf()
        {
            Container.BindFactory<string, Foo, Foo.Factory>().FromComponentInPrefabResource("TestBindFactoryOne/Foo").WithGameObjectName("asdf");

            AddFactoryUser<Foo, Foo.Factory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertNumGameObjectsWithName(Container, "asdf", 1);
        }

        [Test]
        public void TestToPrefabResourceConcrete()
        {
            Container.BindFactory<string, IFoo, IFooFactory>().To<Foo>()
                .FromComponentInPrefabResource("TestBindFactoryOne/Foo").WithGameObjectName("asdf");

            AddFactoryUser<IFoo, IFooFactory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertNumGameObjectsWithName(Container, "asdf", 1);
        }

        [Test]
        public void TestToSubContainerPrefabSelf()
        {
            Container.BindFactory<string, Foo, Foo.Factory>()
                .FromSubContainerResolve().ByPrefab<FooInstaller>(FooSubContainerPrefab);

            AddFactoryUser<Foo, Foo.Factory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestToSubContainerPrefabConcrete()
        {
            Container.BindFactory<string, IFoo, IFooFactory>()
                .To<Foo>().FromSubContainerResolve().ByPrefab<FooInstaller>(FooSubContainerPrefab);

            AddFactoryUser<IFoo, IFooFactory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestToSubContainerPrefabResourceSelf()
        {
            Container.BindFactory<string, Foo, Foo.Factory>()
                .FromSubContainerResolve().ByPrefabResource<FooInstaller>("TestBindFactoryOne/FooSubContainer");

            AddFactoryUser<Foo, Foo.Factory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestToSubContainerPrefabResourceConcrete()
        {
            Container.BindFactory<string, IFoo, IFooFactory>()
                .To<Foo>().FromSubContainerResolve().ByPrefabResource<FooInstaller>("TestBindFactoryOne/FooSubContainer");

            AddFactoryUser<IFoo, IFooFactory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        void AddFactoryUser<TValue, TFactory>()
            where TValue : IFoo
            where TFactory : Factory<string, TValue>
        {
            Container.Bind<IInitializable>()
                .To<FooFactoryTester<TValue, TFactory>>().AsSingle();

            Container.BindExecutionOrder<FooFactoryTester<TValue, TFactory>>(-100);
        }

        public class FooFactoryTester<TValue, TFactory> : IInitializable
            where TFactory : Factory<string, TValue>
            where TValue : IFoo
        {
            readonly TFactory _factory;

            public FooFactoryTester(TFactory factory)
            {
                _factory = factory;
            }

            public void Initialize()
            {
                Assert.IsEqual(_factory.Create("asdf").Value, "asdf");

                Log.Info("Factory created foo successfully");
            }
        }
    }
}
