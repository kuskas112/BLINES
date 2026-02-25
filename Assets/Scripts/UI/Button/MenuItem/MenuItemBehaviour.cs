using UnityEngine;

public class MenuItemBehaviour : ButtonBehaviour
{
    private ButtonFacade facade;
    protected override void Awake()
    {
        base.Awake();
        facade = GetComponent<ButtonFacade>();
    }

    protected override void PlayDefaultAnimation()
    {
        facade.animator.StartGlowToggleAnimation();
        isClicked = false;
    }
    
    protected override void PlaySpecificAnimation()
    {

    }

}
