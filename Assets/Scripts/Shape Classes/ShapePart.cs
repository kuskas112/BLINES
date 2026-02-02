using UnityEngine;
using UnityEngine.U2D;

public class ShapePart : MonoBehaviour
// Часть фигуры Shape.
{
    public SpriteShapeRenderer spriteShapeRenderer;
    public SpriteShapeController spriteShapeController;
    public MaterialSetter materialSetter;
    void Awake()
    {
        spriteShapeRenderer ??= GetComponent<SpriteShapeRenderer>();
        spriteShapeController ??= GetComponent<SpriteShapeController>();
        materialSetter ??= GetComponent<MaterialSetter>();
    }

    private void OnValidate()
    {
        if(
            spriteShapeController == null ||
            spriteShapeRenderer == null ||
            materialSetter == null
        )
        {
            return;
        }
    }
}
