using UnityEngine;

public static class ComponentFactory
{
    public static T CreatePersistent<T>() where T : Component
    {
        var gameObject = new GameObject(typeof(T).Name) { transform = { parent = null } };
        Object.DontDestroyOnLoad(gameObject);
        return gameObject.AddComponent<T>();
    }
}