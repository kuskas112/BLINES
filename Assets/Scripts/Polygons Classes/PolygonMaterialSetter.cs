using UnityEngine;
using UnityEngine.U2D;
using System.Collections;

public class PolygonMaterialSetter : MaterialSetter
{
    private MaterialPropertyBlock propertyBlock;
    public SpriteShapeRenderer shRenderer;

    protected override void Awake()
    {
        propertyBlock = new MaterialPropertyBlock();
        if (!TryGetComponent(out shRenderer))
        {
            Debug.LogError("SpriteShapeRenderer не найден!", this);
            enabled = false;
        }
        
        // По умолчанию берем материал из рендерера
        _edgeMaterial = shRenderer.sharedMaterials[1];
        _edgeNeonColor = EdgeMaterial.GetColor("_NeonColor");
        if (RandomizeColorOnWake)
        {
            EdgeNeonColor = RandomizeColor();
        }
    }

    protected override void UpdateEdgeMaterial()
    {
        Material[] mats = shRenderer.sharedMaterials;
        mats[1] = EdgeMaterial;
        shRenderer.sharedMaterials = mats;
    }

    protected override void UpdateEdgeNeonColor()
    {
        shRenderer.GetPropertyBlock(propertyBlock, 1);
        propertyBlock.SetColor("_NeonColor", EdgeNeonColor);
        shRenderer.SetPropertyBlock(propertyBlock, 1);
    }
}
