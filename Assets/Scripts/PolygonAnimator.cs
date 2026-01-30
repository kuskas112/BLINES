using UnityEngine;
using UnityEngine.U2D;
using System.Collections;
using System.Collections.Generic;

public class PolygonAnimator : MonoBehaviour 
{

    [Header("Animation Settings")]
    public Polygon polygon;
    [SerializeField] private float animationSpeed = 1f;
    [SerializeField] private float pulseAmount = 0.2f;
    [SerializeField] private float rotationSpeed = 0f;
    [SerializeField] private bool autoStartAnimation = true;
    [SerializeField] private float baseRadius = 1f;

    public const string PULSE_ANIMATION_KEY = "pulse";
    public const string ROTATION_ANIMATION_KEY = "rotation";
    
    private Dictionary<string, Coroutine> animationCoroutines = new();


    private void Awake()
    {
        polygon ??= GetComponent<Polygon>();    
    }

    private void Start()
    {
        baseRadius = polygon.Radius;

        if (autoStartAnimation)
        {
            StartDefaultAnimations();
        }
    }

    // ==================== БАЗОВЫЕ АНИМАЦИИ ====================
    
    public void StartDefaultAnimations()
    {
        StopAllAnimations();
        StartAnimation(PULSE_ANIMATION_KEY);
        if (rotationSpeed != 0f)
        {
            StartAnimation(ROTATION_ANIMATION_KEY);
        }
    }
    
    public void StopAllAnimations()
    {
        foreach (var kvp in animationCoroutines)
        {
            if (kvp.Value != null)
            {
                StopCoroutine(kvp.Value);
            }
        }
        animationCoroutines.Clear();
        ResetToBaseShape();
    }
    
    private void ResetToBaseShape()
    {
        polygon.Radius = baseRadius;
        polygon.transform.rotation = Quaternion.identity;
    }

    // ==================== ОСНОВНЫЕ АНИМАЦИОННЫЕ КОРУТИНЫ ====================
    
    private IEnumerator PulseAnimation()
    {
        float time = 0f;
        
        while (true)
        {
            time += Time.deltaTime * animationSpeed;
            float pulse = Mathf.Sin(time) * pulseAmount;
            polygon.Radius = baseRadius * (1 + pulse);
            yield return null;
        }
    }

    private IEnumerator RotationAnimation()
    {
        while (true)
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    // ============================ ПУБЛИЧНОЕ API ДЛЯ АНИМАЦИЙ ================================

    public void StartAnimation(string animationKey)
    {
        if (animationCoroutines.ContainsKey(animationKey))
        {
            // Анимация уже запущена
            return;
        }

        Coroutine animationCoroutine = null;

        switch (animationKey)
        {
            case PULSE_ANIMATION_KEY:
                animationCoroutine = StartCoroutine(PulseAnimation());
                break;
            case ROTATION_ANIMATION_KEY:
                animationCoroutine = StartCoroutine(RotationAnimation());
                break;
            default:
                Debug.LogWarning($"Animation with key {animationKey} not found.");
                return;
        }

        if (animationCoroutine != null)
        {
            animationCoroutines.Add(animationKey, animationCoroutine);
        }
    }

    public void StopAnimation(string animationKey)
    {
        if (animationCoroutines.TryGetValue(animationKey, out Coroutine animationCoroutine))
        {
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
            }
            animationCoroutines.Remove(animationKey);
        }
    }

}
