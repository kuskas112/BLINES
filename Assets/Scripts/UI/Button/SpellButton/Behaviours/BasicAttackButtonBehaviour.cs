using UnityEngine;

public class BasicAttackButtonBehaviour : SpellButtonBehaviour
{
    void Start()
    {
        SetSpell(new BasicAttack());
    }
}
