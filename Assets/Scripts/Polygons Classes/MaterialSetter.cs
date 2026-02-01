using UnityEngine;
using UnityEngine.U2D;
using System.Collections;

public class MaterialSetter : MonoBehaviour
{
    private MaterialPropertyBlock propertyBlock;
    public SpriteShapeRenderer shRenderer;

    [SerializeField]
    private Color _edgeNeonColor;
    public Color EdgeNeonColor { 
        get { return _edgeNeonColor; }
        set
        {
            _edgeNeonColor = value;
            UpdateEdgeNeonColor();
        } 
    }

    [SerializeField]
    private Material _edgeMaterial;
    public Material EdgeMaterial
    {
        get { return _edgeMaterial; }
        set
        {
            _edgeMaterial = value;
            UpdateEdgeMaterial();
        }
    }

    private void Start()
    {
        // По умолчанию берем материал из рендерера
        EdgeMaterial = shRenderer.sharedMaterials[1];
        EdgeNeonColor = EdgeMaterial.GetColor("_NeonColor");
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

    private void UpdateEdgeMaterial()
    {
        Material[] mats = shRenderer.sharedMaterials;
        mats[1] = EdgeMaterial;
        shRenderer.sharedMaterials = mats;
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
        // Долго над проверкой не думал, она все равно только для редактора
        if (shRenderer == null || EdgeMaterial == null || propertyBlock == null) return;
        UpdateEdgeNeonColor();
        // Мне кажется обновлять материал каждый раз избыточно, 
        // из редактора в рантайм это нужно делать крайне редко 
        //UpdateEdgeMaterial();
    }

    // ================== КОРУТИНЫ =======================

    private IEnumerator LerpEdgeNeonColor(Color targetColor, float duration)
    {
        float time = 0f;
        Color startColor = EdgeNeonColor;
        float invDuration = 1f / duration;
        while (time < duration)
        {
            float t = time * invDuration;
            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            Color currentColor = Color.Lerp(startColor, targetColor, smoothT);
            EdgeNeonColor = currentColor;
            time += Time.deltaTime;
            yield return null;
        }
        EdgeNeonColor = targetColor;
    }
}
