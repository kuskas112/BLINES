using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BasicObjectAnimator : MonoBehaviour
{

    public const string ROTATION_ANIMATION_KEY = "rotation";
    public const string SHAKE_ANIMATION_KEY = "shake";
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
    public static IEnumerator RotationAnimation(Transform transform, float speed = 30f)
    {
        while (true)
        {
            transform.Rotate(Vector3.forward, speed * Time.deltaTime);
            yield return null;
        }
    }

    public static IEnumerator ShakeAnimation(Transform transform, float speed = 1f, float shakeOffset = 50f)
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
}
