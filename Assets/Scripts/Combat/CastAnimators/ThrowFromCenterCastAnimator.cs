using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ThrowFromCenterCastAnimator : CastAnimator
{

    public ThrowFromCenterCastAnimator(Spell spell) : base(spell)
    {
    }

    public override IEnumerator CastAnimation(BattleContext context, float duration)
    {
        OnCastAnimationStart.Invoke();
        var buttonFacade = FindSpellButton(context);

        ShapeFacade obj = buttonFacade.behaviour.spellObject.GetComponent<ShapeFacade>();
        Vector3 targetPos = context.IsPlayerTurn ? context.Enemy.PolygonObject.transform.position 
                                                 : context.Player.PolygonObject.transform.position;

        float[] segments = { 0.55f, 0.15f, 0.3f };

        float moveCenterDuration = duration * segments[0];
        float moveTargetDuration = duration * segments[1];
        float moveBackDuration   = duration * segments[2];

        int yCenterPos    = context.IsPlayerTurn ? -3 : 3;
        int rotationAngle = context.IsPlayerTurn ? 0 : 180;

        obj.animator.StopAllAnimations();
        obj.animator.StartRotationChangeAnimation(obj.transform, moveCenterDuration, rotationAngle);

        yield return obj.mover.MoveEaseOut(new(0, yCenterPos), moveCenterDuration);

        yield return obj.mover.MoveEaseOut(targetPos, moveTargetDuration);
        OnHit.Invoke();

        obj.animator.StartRotationChangeAnimation(obj.transform, moveBackDuration, 0);
        Vector3 buttonPos = buttonFacade.behaviour.GetButtonWorldPosition();
        yield return obj.mover.MoveEaseOut(buttonPos, moveBackDuration);
        obj.animator.StartDefaultAnimations();
        context.NextTurn();
        OnCastAnimationEnd.Invoke();
    }
}
