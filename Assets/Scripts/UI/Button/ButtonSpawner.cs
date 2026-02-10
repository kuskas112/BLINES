using UnityEngine;

public class ButtonSpawner : BasicSpawner<ButtonFacade>
{
    public override ButtonFacade Spawn(Vector3 position, Quaternion rotation)
    {
        ButtonFacade instance = base.Spawn(position, rotation);
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
