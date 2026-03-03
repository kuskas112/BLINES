using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShapeAnimator : BasicObjectAnimator
{
    [SerializeField] private bool autoStartAnimation = true;

    [Space()]
    [Header("Default Animation Parameters")]
    [Header("Shake Animation")]
    [SerializeField] private bool shakeEnabled = false;
    [SerializeField] private float shakeSpeed = 1f;
    [SerializeField] private float shakeOffset = 50f;

    [Header("Rotation Animation")]
    [SerializeField] private bool rotationEnabled = false;
    [SerializeField] private float rotationSpeed = 30f;


    public const string BOUNCE_ANIMATION_KEY = "bounce";

    private void Start()
    {
        if (autoStartAnimation)
        {
            StartDefaultAnimations();
        }
    }

    public void StartDefaultAnimations()
    {
        if (shakeEnabled)
        {
            StartShakeAnimation(transform, shakeSpeed, shakeOffset);
        }
        if (rotationEnabled)
        {
            StartRotationAnimation(transform, rotationSpeed);
        }
    }

    private IEnumerator BounceAnimation(float bounceHeight = 0.3f, float bounceDuration = 0.25f, float speed = 1f)
    {
        float time = 0f;
        float invDuration = 1f / bounceDuration; // оказывается умножение быстрее деления в 3-6 раз
        Vector3 startScale = transform.localScale;
        while (time < bounceDuration)
        {
            time += Time.deltaTime * speed;

            // Нормализованное время от 0 до 1
            float t = Mathf.Clamp01(time * invDuration);

            // Формула для bounce эффекта (затухающая синусоида)
            float bounce = Mathf.Sin(t * Mathf.PI * 3) * // 3 колебания
                        Mathf.Pow(1 - t, 2) * // Затухание
                        bounceHeight;

            transform.localScale = startScale * (1 + bounce);
            yield return null;
        }
        transform.localScale = startScale;
        StopBounceAnimation();
    }
    public Coroutine StartBounceAnimation(float bounceHeight = 0.3f, float bounceDuration = 0.25f, float speed = 1f)
    {
        Coroutine animationCoroutine = StartCoroutine(BounceAnimation(bounceHeight, bounceDuration, speed));
        StartAnimation(BOUNCE_ANIMATION_KEY, animationCoroutine);
        return animationCoroutine;
    }
    public void StopBounceAnimation()
    {
        StopAnimation(BOUNCE_ANIMATION_KEY);
    }
}