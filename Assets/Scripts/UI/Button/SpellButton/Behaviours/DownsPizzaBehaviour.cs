using UnityEngine;

public class DownsPizzaBehaviour : SpellButtonBehaviour
{
    public override Spell GetDefaultSpell()
    {
        if (spell == null)
        {
            spell = new DownsPizza();
        }
        return spell;
    }
}
