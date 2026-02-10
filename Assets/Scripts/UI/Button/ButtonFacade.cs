using UnityEngine;

public class ButtonFacade : MonoBehaviour
{
    [HideInInspector] public ButtonMaterialSetter materialSetter;
    [HideInInspector] public ButtonAnimator animator;
    [HideInInspector] public Mover mover;

    protected virtual void Awake()
    {
        materialSetter = GetComponent<ButtonMaterialSetter>();
        animator = GetComponent<ButtonAnimator>();
        mover = GetComponent<Mover>();
    }
}
