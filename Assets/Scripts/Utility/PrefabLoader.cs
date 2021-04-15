using UnityEngine;

public static class PrefabLoader 
{
    public static GameObject LoadPrefab       (string path)                   => Resources.Load<GameObject>(path);
    public static T          CreatePrefabAs<T>(string path, Transform parent) => MonoBehaviour.Instantiate(Resources.Load<GameObject>(path), parent).GetComponent<T>();
}