using UnityEngine;

public abstract class BasicSpawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected T prefab;
    [SerializeField] protected Transform spawnParent;

    public void SetPrefab(T pref)
    {
        prefab = pref;
    }

    public virtual T Spawn(Vector3 position, Quaternion rotation)
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab is not assigned!");
            return null;
        }

        T instance = Instantiate(prefab, position, rotation);

        if (spawnParent != null)
        {
            SetParent(instance, spawnParent);
        }

        OnSpawned(instance);
        return instance;
    }

    protected virtual void OnSpawned(T instance) { }
    protected virtual void SetParent(T instance, Transform parent) {
        // Базовая реализация для 3D объектов
        instance.transform.SetParent(parent);
    }
}