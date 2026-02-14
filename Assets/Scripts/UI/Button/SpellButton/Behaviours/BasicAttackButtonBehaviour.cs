using UnityEngine;

public class BasicAttackBehaviour : SpellButtonBehaviour
{
    void Start()
    {
        SetSpell(new BasicAttack());
    }
}
