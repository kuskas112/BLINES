using UnityEngine;

public class FibonacciAttackBehaviour : SpellButtonBehaviour
{
    public override Spell GetDefaultSpell()
    {
        if (spell == null)
        {
            spell = new FibonacciAttack();
        }
        return spell;
    }
}
