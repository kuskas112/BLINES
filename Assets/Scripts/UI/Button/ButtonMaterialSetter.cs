using UnityEngine;
using UnityEngine.UI;

public class ButtonMaterialSetter : MaterialSetter
{
    public Image image;

    protected override void Awake()
    {
        if(image == null) image = GetComponent<Image>();
        image.material = new Material(image.material);
        _edgeMaterial = image.material;
        if (_edgeNeonColor != Color.clear && !RandomizeColorOnWake)
        {
            // Убираем прозрачность, если она есть, чтобы не было проблем с отображением неона
            if (_edgeNeonColor.a == 0)
            {
                _edgeNeonColor = new Color(
                    _edgeNeonColor.r,
                    _edgeNeonColor.g,
                    _edgeNeonColor.b,
                    100);
            }
            UpdateEdgeNeonColor();
        }
        else
        {
            _edgeNeonColor = image.material.GetColor("_NeonColor");
        }
        base.Awake();
    }
    protected override void UpdateEdgeNeonColor()
    {
        image.material.SetColor("_NeonColor", EdgeNeonColor);
    }
    protected override void UpdateEdgeMaterial()
    {
        image.material = EdgeMaterial;
    }

}
