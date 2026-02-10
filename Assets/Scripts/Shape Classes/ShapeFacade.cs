using UnityEngine;

public class ShapeFacade : MonoBehaviour
{
    [HideInInspector] public Shape shape;
    [HideInInspector] public ShapeAnimator animator;
    [HideInInspector] public Mover mover;

    private void Awake()
    {
        shape    = GetComponent<Shape>();
        animator = GetComponent<ShapeAnimator>();
        mover    = GetComponent<Mover>();
    }
}
