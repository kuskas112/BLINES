using UnityEngine;

public class BlockBehaviour : SpellButtonBehaviour
{
    private void Start()
    {
        SetSpell(new Block());
    }
}
