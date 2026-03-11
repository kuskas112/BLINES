using UnityEngine;

public class SnowballAttackBehaviour : SpellButtonBehaviour
{
    public override Spell GetDefaultSpell()
    {
        if (spell == null)
        {
            spell = new SnowballAttack();
        }
        return spell;
    }
}
