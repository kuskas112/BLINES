using System.Collections;
using UnityEngine;

public class MenuItemBehaviour : ButtonBehaviour
{
    private ButtonFacade facade;
    protected override void Awake()
    {
        base.Awake();
        facade = GetComponent<ButtonFacade>();
    }
    
    protected override void PlaySpecificAnimation()
    {
        isClicked = false;
        StartCoroutine(GlowAndMoveButton(new(0f, -7f)));
    }

    IEnumerator GlowAndMoveButton(Vector2 targetPos) 
    {
        facade.animator.StartGlowToggleAnimation();
        yield return new WaitForSeconds(facade.animator.duration);
        facade.mover.MoveEaseOut(targetPos, 1.5f);
    }

}
