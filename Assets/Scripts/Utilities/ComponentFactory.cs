using UnityEngine;
using static UnityEngine.Object;

public static class ComponentFactory
{
    public static T CreatePersistent<T>() where T : Component
    {
        var gameObject = new GameObject(typeof(T).Name) { transform = { parent = null } };
        DontDestroyOnLoad(gameObject);
        return gameObject.AddComponent<T>();
    }
}