using UnityEngine;

public class FibonacciAttack : BasicAttack
{

    public FibonacciAttack()
    {
        Name = "Fibonacci attack";
        Description = "Deals damage according to Fibonacci sequence";
        this.Type = SpellType.Active;
        this.CastAnimator = new ThrowFromCenterCastAnimator(this);
        Rareness = SpellRareness.Epic;
    }

    int prevAdditive = 0;
    int additive = 1;

    protected override float CalculataDamage(Fighter caster)
    {
        int damage = prevAdditive + additive;
        prevAdditive = additive;
        additive = damage;
        return damage;
    }
}
