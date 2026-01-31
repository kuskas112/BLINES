using UnityEngine;
using UnityEngine.U2D;
using System.Collections;
using System.Collections.Generic;

public class PolygonAnimator : MonoBehaviour 
{

    [Header("Basic Settings")]
    public Polygon polygon;
    [SerializeField] private float baseRadius = 1f;
    [SerializeField] private bool autoStartAnimation = true;

    [Space()]
    [Header("Default Animation Parameters")]

    [Header("Pulse Animation")]
    [SerializeField] private bool pulseEnabled = false;
    [SerializeField] private float pulseSpeed = 1f;
    [SerializeField] private float pulseAmount = 0.2f;

    [Header("Shake Animation")]
    [SerializeField] private bool shakeEnabled = false;
    [SerializeField] private float shakeSpeed = 1f;
    [SerializeField] private float shakeOffset = 50f;

    [Header("Rotation Animation")]
    [SerializeField] private bool rotationEnabled = false;
    [SerializeField] private float rotationSpeed = 30f;


    

    private const string PULSE_ANIMATION_KEY = "pulse";
    private const string ROTATION_ANIMATION_KEY = "rotation";
    private const string MORPH_ANIMATION_KEY = "morph";
    private const string BOUNCE_ANIMATION_KEY = "bounce";
    private const string CHANGE_RADIUS_ANIMATION_KEY = "changeRadius";
    private const string SHAKE_ANIMATION_KEY = "shake";
    
    private Dictionary<string, Coroutine> animationCoroutines = new();


    private void Awake()
    {
        polygon ??= GetComponent<Polygon>();    
    }

    private void Start()
    {
        baseRadius = polygon.Radius;
        
        if (autoStartAnimation) StartDefaultAnimations();
    }

    public void StartDefaultAnimations()
    {
        if (pulseEnabled)
        {
            StartPulseAnimation(pulseSpeed, pulseAmount);
        }
        if (shakeEnabled)
        {
            StartShakeAnimation(shakeSpeed, shakeOffset);
        }
        if (rotationEnabled)
        {
            StartRotationAnimation(rotationSpeed);
        }
    }

    private void OnValidate()
    {
        if (!Application.isPlaying) return;
        if (autoStartAnimation) 
        {
            StopAllAnimations();
            StartDefaultAnimations();
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

    private bool IsAnimationRunning(string animationKey)
    {
        return animationCoroutines.ContainsKey(animationKey);
    }

    // ==================== ОСНОВНЫЕ АНИМАЦИОННЫЕ КОРУТИНЫ ====================
    
    private IEnumerator PulseAnimation(float speed = 1f, float pulseAmount = 0.2f)
    {
        float time = 0f;
        
        while (true)
        {
            time += Time.deltaTime * speed;
            float pulse = Mathf.Sin(time) * pulseAmount;
            polygon.Radius = baseRadius * (1 + pulse);
            yield return null;
        }
    }

    private IEnumerator ShakeAnimation(float speed = 1f, float shakeOffset = 50f)
    {
        float time = 0f;
        
        while (true)
        {
            time += Time.deltaTime * speed;
            float pulse = Mathf.Sin(time) * shakeOffset;
            polygon.transform.rotation = Quaternion.Euler(0, 0, baseRadius * (1 + pulse));
            yield return null;
        }
    }

    private IEnumerator BounceAnimation(float bounceHeight = 0.3f, float bounceDuration = 0.25f, float speed = 1f)
    {
        float time = 0f;
        float startRadius = polygon.Radius;
        
        while (time < bounceDuration)
        {
            time += Time.deltaTime * speed;
            
            // Нормализованное время от 0 до 1
            float t = Mathf.Clamp01(time / bounceDuration);
            
            // Формула для bounce эффекта (затухающая синусоида)
            float bounce = Mathf.Sin(t * Mathf.PI * 3) * // 3 колебания
                        Mathf.Pow(1 - t, 2) * // Затухание
                        bounceHeight;
            
            polygon.Radius = startRadius * (1 + bounce);
            yield return null;
        }

        StopBounceAnimation();
    }

    private IEnumerator ChangeRadiusAnimation(float targetRadius = 2f, float duration = 1f)
    {
        float time = 0f;
        float startRadius = polygon.Radius;
        
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            
            // Плавное ускорение и замедление
            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            polygon.Radius = Mathf.Lerp(startRadius, targetRadius, smoothT);
            
            yield return null;
        }
        
        polygon.Radius = targetRadius;
        
        StopChangeRadiusAnimation();
    }

    private IEnumerator RotationAnimation(float speed = 30f)
    {
        while (true)
        {
            transform.Rotate(Vector3.forward, speed * Time.deltaTime);
            yield return null;
        }
    }

    // Изменение количества углов с Bounce-эффектом
    private IEnumerator MorphAnimation(int targetAngles = 3, float duration = 3f)
    {
        // Почему-то при включенной Pulse-анимации иногда не срабатывает Bounce анимация внутри Morph-а
        // Возможно из-за того, что Pulse постоянно меняет радиус и мешает визуальному эффекту
        // Однако с задержкой все работает корректно

        // Пока уберу, чтобы сохранялось корректное время выполнения анимации, но надо иметь в виду
        // yield return new WaitForSeconds(0.05f); // Небольшая задержка перед началом анимации
        int startAngles = polygon.Angles;
        float time = 0f;
        
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, time / duration);
            
            // Плавное изменение количества сторон
            int currentAngles = Mathf.RoundToInt(Mathf.Lerp(startAngles, targetAngles, t));
            
            if (currentAngles != polygon.Angles)
            {
                StartBounceAnimation();
                polygon.Angles = currentAngles;
            }
            
            yield return null;
        }
        StopMorphAnimation();
    }

    // ============================ ПУБЛИЧНОЕ API ДЛЯ АНИМАЦИЙ ================================

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

    public void StartPulseAnimation(float speed = 1f, float pulseAmount = 0.2f)
    {
        StopAnimation(PULSE_ANIMATION_KEY);
        Coroutine animationCoroutine = StartCoroutine(PulseAnimation(speed, pulseAmount));
        animationCoroutines.Add(PULSE_ANIMATION_KEY, animationCoroutine);
    }
    public void StopPulseAnimation()
    {
        StopAnimation(PULSE_ANIMATION_KEY);
        polygon.Radius = baseRadius;
    }

    public void StartRotationAnimation(float speed = 30f)
    {
        StopAnimation(ROTATION_ANIMATION_KEY);
        Coroutine animationCoroutine = StartCoroutine(RotationAnimation(speed));
        animationCoroutines.Add(ROTATION_ANIMATION_KEY, animationCoroutine);
    }
    public void StopRotationAnimation()
    {
        StopAnimation(ROTATION_ANIMATION_KEY);
        polygon.transform.rotation = Quaternion.identity;
    }

    public void StartMorphAnimation(int targetAngles = 3, float duration = 3f)
    {
        StopAnimation(MORPH_ANIMATION_KEY);
        Coroutine animationCoroutine = StartCoroutine(MorphAnimation(targetAngles, duration));
        animationCoroutines.Add(MORPH_ANIMATION_KEY, animationCoroutine);
    }
    public void StopMorphAnimation()
    {
        StopAnimation(MORPH_ANIMATION_KEY);
    }

    public void StartBounceAnimation(float bounceHeight = 0.3f, float bounceDuration = 0.25f, float speed = 1f)
    {
        StopAnimation(BOUNCE_ANIMATION_KEY);
        Coroutine animationCoroutine = StartCoroutine(BounceAnimation(bounceHeight, bounceDuration, speed));
        animationCoroutines.Add(BOUNCE_ANIMATION_KEY, animationCoroutine);
    }
    public void StopBounceAnimation()
    {
        StopAnimation(BOUNCE_ANIMATION_KEY);
    }

    public void StartChangeRadiusAnimation(float targetRadius = 2f, float duration = 1f)
    {
        StopAnimation(CHANGE_RADIUS_ANIMATION_KEY);
        Coroutine animationCoroutine = StartCoroutine(ChangeRadiusAnimation(targetRadius, duration));
        animationCoroutines.Add(CHANGE_RADIUS_ANIMATION_KEY, animationCoroutine);
    }
    public void StopChangeRadiusAnimation()
    {
        StopAnimation(CHANGE_RADIUS_ANIMATION_KEY);
    }

    public void StartShakeAnimation(float speed = 1f, float shakeOffset = 50f)
    {
        StopAnimation(SHAKE_ANIMATION_KEY);
        Coroutine animationCoroutine = StartCoroutine(ShakeAnimation(speed, shakeOffset));
        animationCoroutines.Add(SHAKE_ANIMATION_KEY, animationCoroutine);
    }
    public void StopShakeAnimation()
    {
        StopAnimation(SHAKE_ANIMATION_KEY);
    }
}
