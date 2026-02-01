using UnityEngine;
using UnityEngine.U2D;
using System.Collections;

public class MaterialSetter : MonoBehaviour
{
    private MaterialPropertyBlock propertyBlock;
    public SpriteShapeRenderer shRenderer;
    public Color EdgeNeonColor;
    public Material EdgeMaterial;

    private void Start()
    {
        EdgeMaterial = shRenderer.sharedMaterials[1];
        EdgeNeonColor = EdgeMaterial.GetColor("_NeonColor");
        StartLerpEdgeNeonColor(Color.purple, 4f);
    }

    void Awake()
    {
        propertyBlock = new MaterialPropertyBlock();
        if (!TryGetComponent(out shRenderer))
        {
            Debug.LogError("SpriteShapeRenderer не найден!", this);
            enabled = false;
        }
    }

    public void SetEdgeMaterial(Material mat)
    {
        EdgeMaterial = mat;
        UpdateEdgeMaterial();
    }
    private void UpdateEdgeMaterial()
    {
        Material[] mats = shRenderer.sharedMaterials;
        mats[1] = EdgeMaterial;
        shRenderer.sharedMaterials = mats;
    }

    public void SetEdgeNeonColor(Color color)
    {
        EdgeNeonColor = color;
        UpdateEdgeNeonColor();
    }
    private void UpdateEdgeNeonColor()
    {
        shRenderer.GetPropertyBlock(propertyBlock, 1);
        propertyBlock.SetColor("_NeonColor", EdgeNeonColor);
        shRenderer.SetPropertyBlock(propertyBlock, 1);
    }

    public void StartLerpEdgeNeonColor(Color targetColor, float duration)
    {
        StartCoroutine(LerpEdgeNeonColor(targetColor, duration));
    }
    public Color RandomizeColor()
    {
        return Random.ColorHSV(0f, 1f, 0.8f, 1f, 1f, 1.5f); // HDR цвета
    }

    private void OnValidate()
    {
        // Для работы в редакторе

        if (shRenderer != null)
        {
            UpdateEdgeNeonColor();
            UpdateEdgeMaterial();
        }
    }


    // ================== КОРУТИНЫ =======================

    private IEnumerator LerpEdgeNeonColor(Color targetColor, float duration)
    {
        float time = 0f;
        Color startColor = EdgeNeonColor;
        while (time < duration)
        {
            float t = time / duration;
            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            Color currentColor = Color.Lerp(startColor, targetColor, smoothT);
            SetEdgeNeonColor(currentColor);
            time += Time.deltaTime;
            yield return null;
        }
        SetEdgeNeonColor(targetColor);
    }
}
