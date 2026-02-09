using UnityEngine;
using UnityEngine.U2D;
using System.Collections;

public class MaterialSetter : MonoBehaviour
{
    [SerializeField]
    protected Color _edgeNeonColor;
    public Color EdgeNeonColor
    {
        get { return _edgeNeonColor; }
        set
        {
            _edgeNeonColor = value;
            UpdateEdgeNeonColor();
        }
    }

    [SerializeField]
    protected Material _edgeMaterial;
    public Material EdgeMaterial
    {
        get { return _edgeMaterial; }
        set
        {
            _edgeMaterial = value;
            UpdateEdgeMaterial();
        }
    }
    public bool RandomizeColorOnWake = false;

    protected virtual void Awake()
    {
        if (RandomizeColorOnWake)
        {
            EdgeNeonColor = RandomizeColor();
        }
    }

    protected virtual void UpdateEdgeMaterial(){}

    protected virtual void UpdateEdgeNeonColor(){}

    public void StartLerpEdgeNeonColor(Color targetColor, float duration)
    {
        StartCoroutine(LerpEdgeNeonColor(targetColor, duration));
    }
    public static Color RandomizeColor()
    {
        return Random.ColorHSV(0f, 1f, 0.8f, 1f, 1f, 1.5f); // HDR ˆ‚ÂÚ‡
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (EdgeMaterial == null) return;
        UpdateEdgeNeonColor();
    }
    #endif

    // ==================  Œ–”“»Õ€ =======================

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
