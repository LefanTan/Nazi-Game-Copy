using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using ModestTree;
using Assert=ModestTree.Assert;
using Zenject.Tests.Factories.BindFactory;

namespace Zenject.Tests.Factories
{
    [TestFixture]
    public class TestBindFactory : ZenjectIntegrationTestFixture
    {
        GameObject FooPrefab
        {
            get { return FixtureUtil.GetPrefab("TestBindFactory/Foo"); }
        }

        GameObject CameraPrefab
        {
            get { return FixtureUtil.GetPrefab("TestBindFactory/Camera"); }
        }

        GameObject FooSubContainerPrefab
        {
            get { return FixtureUtil.GetPrefab("TestBindFactory/FooSubContainer"); }
        }

        [Test]
        public void TestFromNewComponentOnNewGameObject()
        {
            Container.BindFactory<Foo, Foo.Factory>().FromNewComponentOnNewGameObject();

            Initialize();

            FixtureUtil.CallFactoryCreateMethod<Foo, Foo.Factory>(Container);
            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestFromNewComponentOnNewGameObjectComponent()
        {
            Container.BindFactory<Camera, CameraFactory>().FromNewComponentOnNewGameObject();

            Initialize();

            FixtureUtil.CallFactoryCreateMethod<Camera, CameraFactory>(Container);
            FixtureUtil.AssertComponentCount<Camera>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestFromNewComponentOnNewGameObjectComponentFailure()
        {
            Container.BindFactory<string, Camera, CameraFactory2>().FromNewComponentOnNewGameObject();

            Initialize();

            Assert.Throws(() => Container.Resolve<CameraFactory2>().Create("asdf"));
        }

        [Test]
        public void TestFromNewComponentOnNewGameObjectWithParamsSuccess()
        {
            Container.BindFactory<int, Foo2, Foo2.Factory2>().FromNewComponentOnNewGameObject();

            Initialize();

            Container.Resolve<Foo2.Factory2>().Create(5);

            FixtureUtil.AssertComponentCount<Foo2>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestFromNewComponentOnNewGameObjectWithParamsFailure()
        {
            Container.BindFactory<Foo2, Foo2.Factory>().FromNewComponentOnNewGameObject();

            Initialize();

            Assert.Throws(() => Container.Resolve<Foo2.Factory>().Create());
        }

        [Test]
        public void TestFromNewComponentOnNewGameObjectConcrete()
        {
            Container.BindFactory<IFoo, IFooFactory>().To<Foo>().FromNewComponentOnNewGameObject();

            Initialize();

            FixtureUtil.CallFactoryCreateMethod<IFoo, IFooFactory>(Container);
            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestFromNewComponentOnSelf()
        {
            var gameObject = Container.CreateEmptyGameObject("foo");

            Container.BindFactory<Foo, Foo.Factory>().FromNewComponentOn(gameObject);

            Initialize();

            FixtureUtil.CallFactoryCreateMethod<Foo, Foo.Factory>(Container);
            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        [ExpectedException]
        public void TestFromNewComponentOnSelfFail()
        {
            Container.BindFactory<Foo2, Foo2.Factory>().FromNewComponentOn((GameObject)null);

            Initialize();
        }

        [Test]
        public void TestFromNewComponentOnConcrete()
        {
            var gameObject = Container.CreateEmptyGameObject("foo");

            Container.BindFactory<IFoo, IFooFactory>().To<Foo>().FromNewComponentOn(gameObject);

            Initialize();

            FixtureUtil.CallFactoryCreateMethod<IFoo, IFooFactory>(Container);
            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestFromComponentInPrefab()
        {
            Container.BindFactory<Foo, Foo.Factory>().FromComponentInPrefab(FooPrefab).WithGameObjectName("asdf");

            Initialize();

            FixtureUtil.CallFactoryCreateMethod<Foo, Foo.Factory>(Container);

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertNumGameObjectsWithName(Container, "asdf", 1);
        }

        [Test]
        public void TestFromComponentInPrefabComponent()
        {
            Container.BindFactory<Camera, CameraFactory>().FromComponentInPrefab(CameraPrefab).WithGameObjectName("asdf");

            Initialize();

            FixtureUtil.CallFactoryCreateMethod<Camera, CameraFactory>(Container);

            FixtureUtil.AssertComponentCount<Camera>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertNumGameObjectsWithName(Container, "asdf", 1);
        }

        [Test]
        [ExpectedException]
        public void TestToPrefabSelfFail()
        {
            // Foo3 is not on the prefab
            Container.BindFactory<Foo3, Foo3.Factory>().FromComponentInPrefab(FooPrefab);

            Initialize();

            FixtureUtil.CallFactoryCreateMethod<Foo3, Foo3.Factory>(Container);
        }

        [Test]
        public void TestToPrefabConcrete()
        {
            Container.BindFactory<IFoo, IFooFactory>().To<Foo>().FromComponentInPrefab(FooPrefab).WithGameObjectName("asdf");

            Initialize();

            FixtureUtil.CallFactoryCreateMethod<IFoo, IFooFactory>(Container);

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertNumGameObjectsWithName(Container, "asdf", 1);
        }

        [Test]
        public void TestToResourceSelf()
        {
            Container.BindFactory<Texture, Factory<Texture>>()
                .FromResource("TestBindFactory/TestTexture");
            Container.BindRootResolve<Factory<Texture>>();

            Initialize();

            FixtureUtil.CallFactoryCreateMethod<Texture, Factory<Texture>>(Container);
        }

        [Test]
        public void TestToResource()
        {
            Container.BindFactory<UnityEngine.Object, Factory<UnityEngine.Object>>()
                .To<Texture>().FromResource("TestBindFactory/TestTexture");
            Container.BindRootResolve<Factory<UnityEngine.Object>>();

            Initialize();
        }

        [Test]
        public void TestToPrefabResourceSelf()
        {
            Container.BindFactory<Foo, Foo.Factory>().FromComponentInPrefabResource("TestBindFactory/Foo").WithGameObjectName("asdf");

            Initialize();

            FixtureUtil.CallFactoryCreateMethod<Foo, Foo.Factory>(Container);

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertNumGameObjectsWithName(Container, "asdf", 1);
        }

        [Test]
        public void TestToPrefabResourceConcrete()
        {
            Container.BindFactory<Foo, Foo.Factory>().To<Foo>().FromComponentInPrefabResource("TestBindFactory/Foo").WithGameObjectName("asdf");

            Initialize();

            FixtureUtil.CallFactoryCreateMethod<Foo, Foo.Factory>(Container);

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertNumGameObjectsWithName(Container, "asdf", 1);
        }

        [Test]
        public void TestToSubContainerPrefabSelf()
        {
            Container.BindFactory<Foo, Foo.Factory>().FromSubContainerResolve().ByPrefab(FooSubContainerPrefab);

            Initialize();

            FixtureUtil.CallFactoryCreateMethod<Foo, Foo.Factory>(Container);

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestToSubContainerPrefabConcrete()
        {
            Container.BindFactory<IFoo, IFooFactory>()
                .To<Foo>().FromSubContainerResolve().ByPrefab(FooSubContainerPrefab);

            Initialize();

            FixtureUtil.CallFactoryCreateMethod<IFoo, IFooFactory>(Container);
            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestToSubContainerPrefabResourceSelf()
        {
            Container.BindFactory<Foo, Foo.Factory>()
                .FromSubContainerResolve().ByPrefabResource("TestBindFactory/FooSubContainer");

            Initialize();

            FixtureUtil.CallFactoryCreateMethod<Foo, Foo.Factory>(Container);

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestToSubContainerPrefabResourceConcrete()
        {
            Container.BindFactory<IFoo, IFooFactory>()
                .To<Foo>().FromSubContainerResolve().ByPrefabResource("TestBindFactory/FooSubContainer");

            Initialize();

            FixtureUtil.CallFactoryCreateMethod<IFoo, IFooFactory>(Container);
            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestUnderTransformGroup()
        {
            Container.BindFactory<Foo, Foo.Factory>().FromNewComponentOnNewGameObject().UnderTransformGroup("Foo");

            Initialize();

            FixtureUtil.CallFactoryCreateMethod<Foo, Foo.Factory>(Container);

            var root = Container.Resolve<Context>().transform;
            var child1 = root.GetChild(0);

            Assert.IsEqual(child1.name, "Foo");

            var child2 = child1.GetChild(0);

            Assert.IsNotNull(child2.GetComponent<Foo>());
        }

        [Test]
        public void TestUnderTransform()
        {
            var tempGameObject = new GameObject("Foo");

            Container.BindFactory<Foo, Foo.Factory>().FromNewComponentOnNewGameObject().
                UnderTransform(tempGameObject.transform);

            Initialize();

            FixtureUtil.CallFactoryCreateMethod<Foo, Foo.Factory>(Container);

            Assert.IsNotNull(tempGameObject.transform.GetChild(0).GetComponent<Foo>());
        }

        [Test]
        public void TestUnderTransformGetter()
        {
            var tempGameObject = new GameObject("Foo");

            Container.BindFactory<Foo, Foo.Factory>().FromNewComponentOnNewGameObject()
                .UnderTransform((context) => tempGameObject.transform);

            Initialize();

            FixtureUtil.CallFactoryCreateMethod<Foo, Foo.Factory>(Container);

            Assert.IsNotNull(tempGameObject.transform.GetChild(0).GetComponent<Foo>());
        }

        public class CameraFactory2 : Factory<string, Camera>
        {
        }

        public class CameraFactory : Factory<Camera>
        {
        }

        public class Foo3 : MonoBehaviour
        {
            public class Factory : Factory<Foo3>
            {
            }
        }

        public class Foo2 : MonoBehaviour
        {
            [Inject]
            int _value;

            public class Factory : Factory<Foo2>
            {
            }

            public class Factory2 : Factory<int, Foo2>
            {
            }
        }
    }
}
