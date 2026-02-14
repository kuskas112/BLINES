using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class PrefabSelector<T> where T : MonoBehaviour
{

    protected virtual string prefabPath => string.Empty;
    private int _prefabCount = -1;
    public int PrefabCount
    {
        get
        { 
            if(_prefabCount < 0)
            {
                _prefabCount = Resources.LoadAll<T>(prefabPath).Length;
            }
            return _prefabCount;
        }
    }

    private Dictionary<string, T> prefabCache = new();
    private List<string> prefabNames = new();

    protected virtual void Init()
    {
        prefabCache.Clear();
        prefabNames.Clear();
        T[] prefabs = Resources.LoadAll<T>(prefabPath);
        // почему бы и не инициализировать раз уж загрузили
        if(_prefabCount < 0) _prefabCount = prefabs.Length;
        foreach (T prefab in prefabs)
        {
            prefabNames.Add(prefab.gameObject.name);
            Debug.Log("Added prefab " + prefab.gameObject.name);
        }
        Debug.Log("Added all prefabs from " + prefabPath);
    }

    private string GetNameByIndex(int ind)
    {
        return prefabNames[ind];
    }

    public T GetRandomPrefab()
    {
        int randInd = Random.Range(0, PrefabCount); // Исключаем PrefabCount

        string prefabName = GetNameByIndex(randInd);
        if (string.IsNullOrEmpty(prefabName))
        {
            Debug.LogError($"Invalid prefab name at index {randInd}");
            return null;
        }

        if (prefabCache.TryGetValue(prefabName, out T cachedPrefab))
        {
            if (cachedPrefab != null)
            {
                return cachedPrefab;
            }
            else
            {
                // В кэше null - удаляем запись
                prefabCache.Remove(prefabName);
            }
        }

        string path = $"{prefabPath}/{prefabName}";
        T loadedPrefab = Resources.Load<T>(path);

        if (loadedPrefab == null)
        {
            Debug.LogError($"Failed to load prefab at path: {path}");
            return null;
        }

        prefabCache.Add(prefabName, loadedPrefab);

        return loadedPrefab;
    }
}
