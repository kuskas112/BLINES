using UnityEngine;

public class BlockBehaviour : SpellButtonBehaviour
{
    public override Spell GetDefaultSpell()
    {
        if (spell == null)
        {
            spell = new Block();
        }
        return spell;
    }
}
