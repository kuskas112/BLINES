using UnityEngine;

public class RegenerateBehaviour : SpellButtonBehaviour
{

    protected override void Awake()
    {
        base.Awake();
        Castable = false;
    }
    public override Spell GetDefaultSpell()
    {
        if(spell == null)
        {
            spell = new Regenerate();
        }
        return spell;
    }
}
