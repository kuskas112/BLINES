using UnityEngine;

public class SpellButtonFacade : ButtonFacade
{
    [HideInInspector] public SpellButtonBehaviour spellButton;

    protected override void Awake()
    {
        base.Awake();
        spellButton = GetComponent<SpellButtonBehaviour>();
    }
}
