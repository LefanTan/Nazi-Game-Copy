using System;
using System.Collections.Generic;

#if !NOT_UNITY3D
using UnityEngine;
#endif

namespace Zenject
{
    // You can optionally inject this interface into your classes/factories
    // rather than using DiContainer which contains many methods you might not need
    public interface IInstantiator
    {
        // Use this method to create any non-monobehaviour
        // Any fields marked [Inject] will be set using the bindings on the container
        // Any methods marked with a [Inject] will be called
        // Any constructor parameters will be filled in with values from the container
        T Instantiate<T>();
        T Instantiate<T>(IEnumerable<object> extraArgs);

        object Instantiate(Type concreteType);
        object Instantiate(Type concreteType, IEnumerable<object> extraArgs);

#if !NOT_UNITY3D

        // Add new component to existing game object and fill in its dependencies
        // NOTE: Gameobject here is not a prefab prototype, it is an instance
        TContract InstantiateComponent<TContract>(GameObject gameObject)
            where TContract : Component;
        TContract InstantiateComponent<TContract>(
            GameObject gameObject, IEnumerable<object> extraArgs)
            where TContract : Component;
        Component InstantiateComponent(
            Type componentType, GameObject gameObject);
        Component InstantiateComponent(
            Type componentType, GameObject gameObject, IEnumerable<object> extraArgs);

        T InstantiateComponentOnNewGameObject<T>()
            where T : Component;
        T InstantiateComponentOnNewGameObject<T>(string gameObjectName)
            where T : Component;
        T InstantiateComponentOnNewGameObject<T>(IEnumerable<object> extraArgs)
            where T : Component;
        T InstantiateComponentOnNewGameObject<T>(string gameObjectName, IEnumerable<object> extraArgs)
            where T : Component;

        // Create a new game object from a prefab and fill in dependencies for all children
        GameObject InstantiatePrefab(UnityEngine.Object prefab);
        GameObject InstantiatePrefab(
            UnityEngine.Object prefab, string groupName);
        GameObject InstantiatePrefab(
            UnityEngine.Object prefab, GameObjectCreationParameters gameObjectBindInfo);

        // Create a new game object from a resource path and fill in dependencies for all children
        GameObject InstantiatePrefabResource(string resourcePath);
        GameObject InstantiatePrefabResource(
            string resourcePath, string groupName);

        // Same as InstantiatePrefab but returns a component after it's initialized
        // and optionally allows extra arguments for the given component type
        T InstantiatePrefabForComponent<T>(UnityEngine.Object prefab);
        T InstantiatePrefabForComponent<T>(
            UnityEngine.Object prefab, IEnumerable<object> extraArgs);
        object InstantiatePrefabForComponent(
            Type concreteType, UnityEngine.Object prefab, IEnumerable<object> extraArgs);

        // Same as InstantiatePrefabResource but returns a component after it's initialized
        // and optionally allows extra arguments for the given component type
        T InstantiatePrefabResourceForComponent<T>(string resourcePath);
        T InstantiatePrefabResourceForComponent<T>(
            string resourcePath, IEnumerable<object> extraArgs);
        object InstantiatePrefabResourceForComponent(
            Type concreteType, string resourcePath, IEnumerable<object> extraArgs);

        T InstantiateScriptableObjectResource<T>(string resourcePath)
            where T : ScriptableObject;
        T InstantiateScriptableObjectResource<T>(
            string resourcePath, IEnumerable<object> extraArgs)
            where T : ScriptableObject;
        object InstantiateScriptableObjectResource(
            Type scriptableObjectType, string resourcePath);
        object InstantiateScriptableObjectResource(
            Type scriptableObjectType, string resourcePath, IEnumerable<object> extraArgs);

        GameObject CreateEmptyGameObject(string name);
#endif

        // The below explicit methods are mostly meant for internal use
        // but may be useful to you too if you need to pass in null value arguments

        T InstantiateExplicit<T>(List<TypeValuePair> extraArgs);
        object InstantiateExplicit(Type concreteType, List<TypeValuePair> extraArgs);
        object InstantiateExplicit(Type concreteType, bool autoInject, InjectArgs args);

#if !NOT_UNITY3D
        Component InstantiateComponentExplicit(
            Type componentType, GameObject gameObject, List<TypeValuePair> extraArgs);

        object InstantiateScriptableObjectResourceExplicit(
            Type scriptableObjectType, string resourcePath, List<TypeValuePair> extraArgs);

        // Same as InstantiatePrefabResourceForComponent except allows null values
        // to be included in the argument list.  Also see InjectUtil.CreateArgList
        T InstantiatePrefabResourceForComponentExplicit<T>(
            string resourcePath, List<TypeValuePair> extraArgs);
        object InstantiatePrefabResourceForComponentExplicit(
            Type concreteType, string resourcePath, List<TypeValuePair> extraArgs);
        object InstantiatePrefabResourceForComponentExplicit(
            Type concreteType, string resourcePath, string groupName, InjectArgs args);

        // Same as InstantiatePrefabForComponent except allows null values
        // to be included in the argument list.  Also see InjectUtil.CreateArgList
        T InstantiatePrefabForComponentExplicit<T>(
            UnityEngine.Object prefab, List<TypeValuePair> extraArgs);
        object InstantiatePrefabForComponentExplicit(
            Type componentType, UnityEngine.Object prefab, List<TypeValuePair> extraArgs);
        object InstantiatePrefabForComponentExplicit(
            Type componentType, UnityEngine.Object prefab, List<TypeValuePair> extraArgs,
            string groupName);
        object InstantiatePrefabForComponentExplicit(
            Type componentType, UnityEngine.Object prefab, string groupName, InjectArgs args);
        object InstantiatePrefabForComponentExplicit(
            Type componentType, UnityEngine.Object prefab,
            GameObjectCreationParameters gameObjectBindInfo, InjectArgs args);
#endif

    }
}

