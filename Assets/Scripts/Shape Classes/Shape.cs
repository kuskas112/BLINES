using UnityEngine;

public class Shape : MonoBehaviour
// Необходимы не только идеальные равносторонние полигоны как Polygon.
// Функционал для произвольных фигур сильно отличаться, чтобы они наследовались от Polygon.cs.

// Shape будет представлять из себя набор ShapePart-ов (частей фигуры).
{
    public ShapePart[] shapeParts;

    protected void Awake()
    {
        shapeParts = GetComponentsInChildren<ShapePart>();
    }

    public void SetColorAllParts(Color color)
    {
        foreach (var part in shapeParts)
        {
            part.materialSetter.EdgeNeonColor = color;
        }
    }

    public void SetColorPart(int index, Color color)
    {
        if(index < 0 || index >= shapeParts.Length) throw new System.IndexOutOfRangeException("Invalid shape part index.");
        shapeParts[index].materialSetter.EdgeNeonColor = color;
    }
}
