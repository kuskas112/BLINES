using UnityEngine;

public class SpellButtonFacade : ButtonFacade
{
    [HideInInspector] public SpellButtonBehaviour behaviour;

    protected override void Awake()
    {
        base.Awake();
        behaviour = GetComponent<SpellButtonBehaviour>();
    }
}
