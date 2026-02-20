using UnityEngine;

public class SpellButtonFacade : ButtonFacade
{
    [HideInInspector] public SpellButtonBehaviour behaviour;
    private bool isAwaken = false;

    protected override void Awake()
    {
        if (isAwaken) return;
        base.Awake();
        behaviour = GetComponent<SpellButtonBehaviour>();
    }

    // Для спавна, т.к. Awake сам по себе вызывается не при спавне а при инстанцировании в сцене,
    // а компоненты получаемые в нем могут понадобиться еще до этого события
    public void Init()
    {
        Awake();
        isAwaken = true;
    }
}
