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

    protected virtual T[] Init()
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
        return prefabs; // возврат загруженных префабов для работы с ними в наследниках
    }

    private string GetNameByIndex(int ind)
    {
        return prefabNames[ind];
    }

    public T GetPrefabByName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError($"Invalid prefab name");
            return null;
        }

        if (prefabCache.TryGetValue(name, out T cachedPrefab))
        {
            if (cachedPrefab != null)
            {
                return cachedPrefab;
            }
            else
            {
                // В кэше null - удаляем запись
                prefabCache.Remove(name);
            }
        }

        string path = $"{prefabPath}/{name}";
        T loadedPrefab = Resources.Load<T>(path);

        if (loadedPrefab == null)
        {
            Debug.LogError($"Failed to load prefab at path: {path}");
            return null;
        }

        prefabCache.Add(name, loadedPrefab);
        return loadedPrefab;
    }

    public T GetRandomPrefab()
    {
        int randInd = Random.Range(0, PrefabCount); // Исключаем PrefabCount
        string prefabName = GetNameByIndex(randInd);
        return GetPrefabByName(prefabName);
    }
}
