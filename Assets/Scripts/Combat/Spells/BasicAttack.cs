using System.Collections;
using UnityEngine;
public class BasicAttack : Spell
{
    public BasicAttack()
    {
        Name              = "Basic Attack";
        Description       = "Deals damage to the target based on angle count";
        this.Type         = SpellType.Active; 
        this.CastAnimator = new ThrowFromCenterCastAnimator(this);
    }

    public override void Cast(BattleContext context)
    {
        base.Cast(context);
        CastAnimator.OnHit.RemoveAllListeners();
        Fighter target = context.IsPlayerTurn ? context.Enemy : context.Player;
        Fighter caster = context.IsPlayerTurn ? context.Player : context.Enemy;
        
        float damage = CalculataDamage(caster);
        foreach (var modifier in Modifiers)
        {
            if(modifier.Target == ModifierTarget.AttackDamage)
            {
                damage = modifier.Apply(damage);
            }
        }
        CastAnimator.OnHit.AddListener(() => target.TakeDamage(damage));
        CastAnimator.StartCastAnimation(context, 1.5f);
    }

    protected virtual float CalculataDamage(Fighter caster)
    {
        return caster.GetPolygon().Angles;
    }
}
