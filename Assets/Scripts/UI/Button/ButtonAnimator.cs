using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ButtonAnimator : BasicObjectAnimator
{
    public const string GLOW_TOGGLE_ANIMATION_KEY = "glowToggle";
    public const string CHANGE_SHAPE_ANIMATION_KEY = "changeShape";

    protected MaterialSetter buttonMaterialSetter;
    protected RectTransform buttonRect;
    protected Color baseColor;

    [Header("Glow Animation Settings")]
    public float startGlow = 1f;
    public float maxGlow = 1.5f;
    public float duration = 0.2f;

    private void Awake()
    {
        buttonMaterialSetter = GetComponent<MaterialSetter>();
        buttonRect = GetComponent<RectTransform>();
    }

    public virtual void StartOnClickAnimation()
    {
        StartGlowToggleAnimation(maxGlow, duration);
    }

    public IEnumerator GlowToggleAnimation(float maxGlow = 0.5f, float duration = 1f)
    {
        float time = 0f;
        //float startGlow = 1f;
        float invDuration = 1 / duration;
        Color localBaseColor = buttonMaterialSetter.EdgeNeonColor;
        while (time < duration / 2)
        {
            time += Time.deltaTime;
            float t = time * invDuration;
            float smoothT = Mathf.SmoothStep(startGlow, maxGlow, t);
            buttonMaterialSetter.EdgeNeonColor = localBaseColor * smoothT;
            yield return null;
        }

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time * invDuration;
            float smoothT = Mathf.SmoothStep(maxGlow, startGlow, t);
            buttonMaterialSetter.EdgeNeonColor = localBaseColor * smoothT;
            yield return null;
        }

        buttonMaterialSetter.EdgeNeonColor = baseColor; // Убедиться, что цвет сброшен в исходное состояние
        StopGlowToggleAnimation();
    }
    public void StartGlowToggleAnimation(float maxGlow = 0.5f, float duration = 1f)
    {
        if (baseColor == Color.clear) baseColor = buttonMaterialSetter.EdgeNeonColor;
        Coroutine animationCoroutine = StartCoroutine(GlowToggleAnimation(maxGlow, duration));
        StartAnimation(GLOW_TOGGLE_ANIMATION_KEY, animationCoroutine);
    }
    public void StopGlowToggleAnimation()
    {
        StopAnimation(GLOW_TOGGLE_ANIMATION_KEY);
    }


    public IEnumerator ChangeShapeAnimation(Vector2 targetSize, float duration = 1f)
    {
        float time = 0f;
        float invDuration = 1 / duration;
        Vector2 startSize = buttonRect.sizeDelta;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time * invDuration;
            float smoothT = Mathf.SmoothStep(0, 1, t);
            buttonRect.sizeDelta = Vector2.Lerp(startSize, targetSize, smoothT);
            yield return null;
        }
        buttonRect.sizeDelta = targetSize; // Убедиться, что размер сброшен в целевое состояние
        StopChangeShapeAnimation();
    }
    public void StartChangeShapeAnimation(Vector2 targetSize, float duration = 1f)
    {
        Coroutine animationCoroutine = StartCoroutine(ChangeShapeAnimation(targetSize, duration));
        StartAnimation(CHANGE_SHAPE_ANIMATION_KEY, animationCoroutine);
    }
    public void StopChangeShapeAnimation()
    {
        StopAnimation(CHANGE_SHAPE_ANIMATION_KEY);
    }
}
