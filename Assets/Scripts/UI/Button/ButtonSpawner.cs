using UnityEngine;

public abstract class ButtonSpawner<T> : BasicSpawner<T> where T : ButtonFacade
{
    public override T Spawn(Vector3 position, Quaternion rotation)
    {
        T instance = base.Spawn(position, rotation);
        // Изменения в позиции для UI элементов
        instance.gameObject.transform.localScale = Vector3.one;
        if (instance.TryGetComponent<RectTransform>(out var rectTransform))
        {
            // Исправляем Z координату через localPosition
            Vector3 localPos = rectTransform.localPosition;
            localPos.z = 0;
            rectTransform.localPosition = localPos;
        }
        return instance;
    }
}
