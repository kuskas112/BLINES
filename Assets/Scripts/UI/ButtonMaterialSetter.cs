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
        _edgeNeonColor = image.material.GetColor("_NeonColor");
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
