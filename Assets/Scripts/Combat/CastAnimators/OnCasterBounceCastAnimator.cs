using System.Collections;
using UnityEngine;

public class OnCasterBounceCastAnimator : CastAnimator
{
    public OnCasterBounceCastAnimator(Spell spell) : base(spell)
    {
    }

    public override IEnumerator CastAnimation(BattleContext context, float duration)
    {
        OnCastAnimationStart.Invoke();
        var buttonFacade = FindSpellButton(context);

        ShapeFacade obj = buttonFacade.behaviour.spellObject.GetComponent<ShapeFacade>();
        Vector3 targetPos = context.IsPlayerTurn ? context.Player.PolygonObject.transform.position
                                                 : context.Enemy.PolygonObject.transform.position;

        float[] segments = { 0.3f, 0.4f, 0.3f };

        float moveToTargetDuration = duration * segments[0];
        float applyDuration = duration * segments[1];
        float moveBackDuration = duration * segments[2];


        obj.animator.StopAllAnimations();

        yield return obj.mover.MoveEaseOut(targetPos, moveToTargetDuration);

        yield return obj.animator.StartBounceAnimation(0.3f, applyDuration);
        OnHit.Invoke();

        Vector3 buttonPos = buttonFacade.behaviour.GetButtonWorldPosition();
        yield return obj.mover.MoveEaseOut(buttonPos, moveBackDuration);
        
        obj.animator.StartDefaultAnimations();
        context.NextTurn();
        OnCastAnimationEnd.Invoke();
    }
}
