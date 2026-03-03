using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BasicObjectAnimator : MonoBehaviour
{

    public const string ROTATION_ANIMATION_KEY = "rotation";
    public const string SHAKE_ANIMATION_KEY = "shake";
    public const string ROTATION_CHANGE_ANIMATION_KEY = "rotationChange";
    public const string SCALE_TO_ANIMATION_KEY = "ScaleTo";

    protected Dictionary<string, Coroutine> animationCoroutines = new();

    public virtual void StartAnimation(string animationKey, Coroutine animationCoroutine)
    {
        StopAnimation(animationKey);
        animationCoroutines.Add(animationKey, animationCoroutine);
    }

    public virtual void StopAnimation(string animationKey)
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

    public virtual void StopAllAnimations()
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
    
    protected virtual void ResetToBaseShape(){}

    public virtual bool IsAnimationRunning(string animationKey)
    {
        return animationCoroutines.ContainsKey(animationKey);
    }

    // =================== КОРУТИНЫ =======================
    public IEnumerator RotationAnimation(Transform transform, float speed = 30f)
    {
        while (true)
        {
            transform.Rotate(Vector3.forward, speed * Time.deltaTime);
            yield return null;
        }
    }

    public IEnumerator ScaleTo(Transform target, Vector3 targetScale, float duration)
    {
        Vector3 startScale = target.localScale;
        float elapsed = 0f;
        float invDuration = 1 / duration;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed * invDuration;
            float smoothT = Mathf.SmoothStep(0, 1, t);
            target.localScale = Vector3.Lerp(startScale, targetScale, smoothT);
            yield return null;
        }
        target.localScale = targetScale;
    }

    public IEnumerator ShakeAnimation(Transform transform, float speed = 1f, float shakeOffset = 50f)
    {
        float time = 0f;
        
        while (true)
        {
            time += Time.deltaTime * speed;
            float pulse = Mathf.Sin(time) * shakeOffset;
            transform.rotation = Quaternion.Euler(0, 0, 1 + pulse);
            yield return null;
        }
    }

    // Не постоянное вращение, а постановка в определенный угол с анимацией
    public IEnumerator RotationChangeAnimation(Transform transform, float duration = 1f, float targetAngle = 0f)
    {
        float time = 0f;
        float startAngle = transform.rotation.eulerAngles.z;
        float invDuration = 1 / duration;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time * invDuration;
            float smoothT = Mathf.SmoothStep(startAngle, targetAngle, t);
            //float currentAngle = Mathf.LerpAngle(startAngle, targetAngle, smoothT);
            transform.rotation = Quaternion.Euler(0, 0, smoothT);
            yield return null;
        }
        StopRotationChangeAnimation();
    }

    // ====================== ПУБЛИЧНОЕ API ============================
    public Coroutine StartRotationAnimation(Transform transform, float speed = 30f)
    {
        Coroutine animationCoroutine = StartCoroutine(RotationAnimation(transform, speed));
        StartAnimation(ROTATION_ANIMATION_KEY, animationCoroutine);
        return animationCoroutine;
    }
    public void StopRotationAnimation()
    {
        StopAnimation(ROTATION_ANIMATION_KEY);
    }

    public Coroutine StartShakeAnimation(Transform transform, float speed = 1f, float shakeOffset = 50f)
    {
        Coroutine animationCoroutine = StartCoroutine(ShakeAnimation(transform, speed, shakeOffset));
        StartAnimation(SHAKE_ANIMATION_KEY, animationCoroutine);
        return animationCoroutine;
    }
    public void StopShakeAnimation()
    {
        StopAnimation(SHAKE_ANIMATION_KEY);
    }

    public Coroutine StartRotationChangeAnimation(Transform transform, float duration = 1f, float targetAngle = 0f)
    {
        Coroutine animationCoroutine = StartCoroutine(RotationChangeAnimation(transform, duration, targetAngle));
        StartAnimation(ROTATION_CHANGE_ANIMATION_KEY, animationCoroutine);
        return animationCoroutine;
    }
    public void StopRotationChangeAnimation()
    {
        StopAnimation(ROTATION_CHANGE_ANIMATION_KEY);
    }

    public Coroutine StartScaleTo(Transform target, Vector3 targetScale, float duration)
    {
        Coroutine animationCoroutine = StartCoroutine(ScaleTo(target, targetScale, duration));
        StartAnimation(SCALE_TO_ANIMATION_KEY, animationCoroutine);
        return animationCoroutine;
    }
    public void StopScaleTo()
    {
        StopAnimation(SCALE_TO_ANIMATION_KEY);
    }

}
