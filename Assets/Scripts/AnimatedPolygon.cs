using UnityEngine;
using UnityEngine.U2D;
using System.Collections;
public class AnimatedPolygon : Polygon
{

    [Header("Animation Settings")]
    [SerializeField] protected float animationSpeed = 1f;
    [SerializeField] protected float pulseAmount = 0.2f;
    [SerializeField] protected float rotationSpeed = 0f;
    [SerializeField] protected bool autoStartAnimation = true;
    
    protected Coroutine currentAnimation;
    protected bool isAnimating = false;
    protected float baseRadius = 1f;

    protected virtual void Update()
    {
        if (rotationSpeed != 0f)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }

    protected override void Start()
    {
        base.Start();
        baseRadius = Radius;

        if (autoStartAnimation)
        {
            StartDefaultAnimation();
        }
    }

    // ==================== БАЗОВЫЕ АНИМАЦИИ ====================
    
    public virtual void StartDefaultAnimation()
    {
        StopAllAnimations();
        currentAnimation = StartCoroutine(PulseAnimation());
    }
    
    public virtual void StopAllAnimations()
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
            currentAnimation = null;
        }
        isAnimating = false;
        ResetToBaseShape();
    }
    
    protected virtual void ResetToBaseShape()
    {
        Radius = baseRadius;
    }

    // ==================== ОСНОВНЫЕ АНИМАЦИОННЫЕ КОРУТИНЫ ====================
    
    protected virtual IEnumerator PulseAnimation()
    {
        isAnimating = true;
        float time = 0f;
        
        while (isAnimating)
        {
            time += Time.deltaTime * animationSpeed;
            float pulse = Mathf.Sin(time) * pulseAmount;
            Radius = baseRadius * (1 + pulse);
            yield return null;
        }
    }

}
