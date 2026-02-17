using UnityEngine;

public class BasicAttackBehaviour : SpellButtonBehaviour
{
    public override Spell GetDefaultSpell()
    {
        if(spell == null)
        {
            spell = new BasicAttack();
        }
        return spell;
    }
}
