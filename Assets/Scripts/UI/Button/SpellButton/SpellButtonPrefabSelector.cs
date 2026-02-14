using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class PrefabSelector<T> : MonoBehaviour where T : MonoBehaviour
{
    private static PrefabSelector<T> _instance;
    public static PrefabSelector<T> Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Object.FindFirstObjectByType<PrefabSelector<T>>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("PrefabSelector");
                    _instance = go.AddComponent<PrefabSelector<T>>();
                }
                _instance.Init();
            }
            return _instance;
        }
    }

    public string prefabPath;
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

    private void Init()
    {
        prefabCache.Clear();
        prefabNames.Clear();
        T[] prefabs = Resources.LoadAll<T>(prefabPath);
        // почему бы и не инициализировать раз уж загрузили
        if(_prefabCount < 0) _prefabCount = prefabs.Length;
        foreach (T prefab in prefabs)
        {
            prefabNames.Add(prefab.gameObject.name);
        }
    }

    private string GetNameByIndex(int ind)
    {
        return prefabNames[ind];
    }

    public T GetRandomPrefab()
    {
        int randInd = Random.Range(0, PrefabCount + 1);
        string prefabName = GetNameByIndex(randInd);
        T inst;

        if (prefabCache.TryGetValue(prefabName, out inst))
        {
            if (inst != null)
            {
                return inst;
            }
        }

        inst = Resources.Load<T>($"{prefabPath}/{prefabName}");
        prefabCache.Add(prefabName, inst);
        return inst;
    }
}
