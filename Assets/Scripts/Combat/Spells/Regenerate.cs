using System.Collections;
using UnityEngine;
public class Regenerate : Spell
{
    public Regenerate()
    {
        Name              = "Regenerate";
        Description       = $"Heals {SpellDescriptionBehaviour.ColorString("Angle / 2 HP", Color.green)} to player every round";
        this.Type         = SpellType.Passive;
        this.Rareness     = SpellRareness.Rare; 
        this.CastAnimator = new InPlaceRotateCastAnimator(this);
        CastAnimator.NeedToSwitchTurn = false;
    }

    public override void Cast(BattleContext context)
    {
        base.Cast(context);
        Fighter caster = context.IsPlayerTurn ? context.Player : context.Enemy;

        float heal = CalculataHeal(caster);
        foreach (var modifier in Modifiers)
        {
            if(modifier.Target == ModifierTarget.HealingAmount)
            {
                heal = modifier.Apply(heal);
            }
        }
        caster.TakeHeal(heal);
        
        CastAnimator?.StartCastAnimation(context, 1.5f);
    }

    protected virtual float CalculataHeal(Fighter caster)
    {
        return caster.GetPolygon().Angles / 2f;
    }
}
