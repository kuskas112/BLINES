using UnityEngine;

public class ShapeFacade
{
    public Shape shape;
    public ShapeAnimator animator;
    public Mover mover;

    public ShapeFacade(GameObject shapeObject)
    {
        shape = shapeObject.GetComponent<Shape>();
        animator = shapeObject.GetComponent<ShapeAnimator>();
        mover = shapeObject.gameObject.GetComponent<Mover>();
    }
}
