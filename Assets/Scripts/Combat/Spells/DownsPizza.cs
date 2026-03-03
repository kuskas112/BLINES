using UnityEngine;
public class DownsPizza : Spell
{
    public DownsPizza()
    {
        Name = "Down`s Pizza";
        Description = "Healing few HP";
        this.Type = SpellType.Consumable; 
        Rareness = SpellRareness.Epic;
        CastAnimator = new OnCasterBounceCastAnimator(this);
    }
    public float HealMultiplier = 2.5f;

    public override void Cast(BattleContext context)
    {
        base.Cast(context);
        CastAnimator.OnHit.RemoveAllListeners();
        Fighter caster = context.IsPlayerTurn ? context.Player : context.Enemy;
        
        float amount = caster.GetPolygon().Angles * HealMultiplier;
        foreach (var modifier in Modifiers)
        {
            if(modifier.Target == ModifierTarget.HealingAmount)
            {
                amount = modifier.Apply(amount);
            }
        }

        CastAnimator.OnHit.AddListener(() => caster.TakeHeal(amount));
        CastAnimator?.StartCastAnimation(context, 2.5f);
    }
}
