using UnityEngine;

public class SnowballAttack : BasicAttack
{

    public SnowballAttack()
    {
        Name = "Snowball attack";
        Description = "Deals +1 damage every cast";
        this.Type = SpellType.Active;
        this.CastAnimator = new ThrowFromCenterCastAnimator(this);
        Rareness = SpellRareness.Rare;
    }

    int InARowUses = 0;
    protected override float CalculataDamage(Fighter caster)
    {
        return base.CalculataDamage(caster) + InARowUses++;
    }
}
