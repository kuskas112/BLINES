using System.Collections;
using UnityEngine;

public class InPlaceRotateCastAnimator : CastAnimator
{
    public InPlaceRotateCastAnimator(Spell spell) : base(spell)
    {
    }

    public override IEnumerator CastAnimation(BattleContext context, float duration)
    {
        OnCastAnimationStart.Invoke();
        var buttonFacade = FindSpellButton(context);

        ShapeFacade obj = buttonFacade.behaviour.spellObject.GetComponent<ShapeFacade>();

        obj.animator.StopAllAnimations();
        obj.transform.rotation = Quaternion.identity;
        var baseRotation = obj.transform.rotation;

        yield return obj.animator.StartRotationChangeAnimation(obj.transform, duration / 2f, baseRotation.z + 360f);


        obj.transform.rotation = baseRotation;
        obj.animator.StartDefaultAnimations();
        if (NeedToSwitchTurn) context.NextTurn();
        OnCastAnimationEnd.Invoke();
    }
}
