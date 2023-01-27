using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityUtils
{
    /// <summary>
    /// Will instantiate an object disabled preventing it from calling Awake/OnEnable.
    /// </summary>
    public static T InstantiateDisabled<T>(T original, Transform parent = null, bool worldPositionStays = false) where T : Object
    {
        if (!GetActiveState(original))
        {
            return Object.Instantiate(original, parent, worldPositionStays);
        }

        (GameObject coreObject, Transform coreObjectTransform) = CreateDisabledCoreObject(parent);
        T instance = Object.Instantiate(original, coreObjectTransform, worldPositionStays);
        SetActiveState(instance, false);
        SetParent(instance, parent, worldPositionStays);
        Object.Destroy(coreObject);
        return instance;
    }

    /// <summary>
    /// Will instantiate an object disabled preventing it from calling Awake/OnEnable.
    /// </summary>
    public static T InstantiateDisabled<T>(T original, Vector3 position, Quaternion rotation, Transform parent = null) where T : Object
    {
        if (!GetActiveState(original))
        {
            return Object.Instantiate(original, position, rotation, parent);
        }

        (GameObject coreObject, Transform coreObjectTransform) = CreateDisabledCoreObject(parent);
        T instance = Object.Instantiate(original, position, rotation, coreObjectTransform);
        SetActiveState(instance, false);
        SetParent(instance, parent, false);
        Object.Destroy(coreObject);
        return instance;
    }

    private static (GameObject coreObject, Transform coreObjectTransform) CreateDisabledCoreObject(Transform parent = null)
    {
        GameObject coreObject = new GameObject(string.Empty);
        coreObject.SetActive(false);
        Transform coreObjectTransform = coreObject.transform;
        coreObjectTransform.SetParent(parent);

        return (coreObject, coreObjectTransform);
    }

    public static bool GetActiveState<T>(T @object) where T : Object
    {
        switch (@object)
        {
            case GameObject gameObject:
                {
                    return gameObject.activeSelf;
                }
            case Component component:
                {
                    return component.gameObject.activeSelf;
                }
            default:
                {
                    return false;
                }
        }
    }

    public static bool GetActiveInHierachyState<T>(T @object) where T : Object
    {
        switch (@object)
        {
            case GameObject gameObject:
                {
                    return gameObject.activeInHierarchy;
                }
            case Component component:
                {
                    return component.gameObject.activeInHierarchy;
                }
            default:
                {
                    return false;
                }
        }
    }

    public static void SetActiveState<T>(T @object, bool state) where T : Object
    {
        switch (@object)
        {
            case GameObject gameObject:
                {
                    gameObject.SetActive(state);

                    break;
                }
            case Component component:
                {
                    component.gameObject.SetActive(state);

                    break;
                }
        }
    }

    public static void SetParent<T>(T @object, Transform parent, bool worldPositionStays) where T : Object
    {
        switch (@object)
        {
            case GameObject gameObject:
                {
                    gameObject.transform.SetParent(parent, worldPositionStays);

                    break;
                }
            case Component component:
                {
                    component.transform.SetParent(parent, worldPositionStays);

                    break;
                }
        }
    }

    public static void SetTransform<T>(T @object, Vector3 position, Quaternion rotation) where T : Object
    {
        switch (@object)
        {
            case GameObject gameObject:
                {
                    gameObject.transform.position = position;
                    gameObject.transform.rotation = rotation;

                    break;
                }
            case Component component:
                {
                    component.transform.position = position;
                    component.transform.rotation = rotation;

                    break;
                }
        }
    }
}