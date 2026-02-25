using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


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
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("GameScene");
    }

}
