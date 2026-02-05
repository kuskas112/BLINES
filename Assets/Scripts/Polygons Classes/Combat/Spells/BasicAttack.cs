using UnityEngine;

public class BasicAttack : Spell
{
    public BasicAttack()
    {
        Name = "Basic Attack";
        Description = "Deals damage to the target based on angle count";
        this.Type = SpellType.Active;
    }

    public override void Cast(BattleContext context)
    {
        base.Cast(context);
        Fighter target = context.IsPlayerTurn ? context.Enemy : context.Player;
        Fighter caster = context.IsPlayerTurn ? context.Player : context.Enemy;
        
        float damage = caster.GetPolygon().Angles;
        foreach (var modifier in Modifiers)
        {
            damage = modifier.Apply(damage);
        }

        target.TakeDamage(damage);
    }
}
