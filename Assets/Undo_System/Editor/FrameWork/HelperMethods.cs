using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class HelperMethods
{
    /// <summary>
    /// Get Derived Types of T in Assembly
    /// </summary>
    public static List<Type> GetDerivedTypes<T>() where T : class
    {
        var type = typeof(T);

        List<Type> derivedClasses = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => x.IsSubclassOf(type))
            .ToList();

        return derivedClasses; //Returning Types
    }


    /// <summary>
    /// Determines if we are in Edit mode or not.
    /// </summary>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    public static bool InEditMode()
    {
        if (!EditorApplication.isPlaying && !EditorApplication.isPaused) return true;

        return false;
    }

    /// <summary>
    /// Determines whether the specified game object is a prefab.
    /// </summary>
    /// <param name="gameObject">The game object.</param>
    /// <returns><c>true</c> if [is a_ prefab] [the specified game object]; otherwise, <c>false</c>.</returns>
    public static bool IsA_Prefab(GameObject gameObject)
    {
        return
            PrefabUtility.GetPrefabType(gameObject) == PrefabType.Prefab ||
            PrefabUtility.GetPrefabType(gameObject) == PrefabType.ModelPrefab;

    }
}