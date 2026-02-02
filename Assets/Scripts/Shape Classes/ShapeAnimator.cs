using UnityEngine;

public class ShapeAnimator : BasicObjectAnimator
{

    private void Start()
    {
        StartShakeAnimation(transform, 1f, 50f);
    }
    public void StartRotationAnimation(Transform transform, float speed = 30f)
    {
        Coroutine animationCoroutine = StartCoroutine(BasicObjectAnimator.RotationAnimation(transform, speed));
        StartAnimation(ROTATION_ANIMATION_KEY, animationCoroutine);
    }
    public void StopRotationAnimation()
    {
        StopAnimation(ROTATION_ANIMATION_KEY);
    }

    public void StartShakeAnimation(Transform transform, float speed = 1f, float shakeOffset = 50f)
    {
        Coroutine animationCoroutine = StartCoroutine(BasicObjectAnimator.ShakeAnimation(transform, speed, shakeOffset));
        StartAnimation(SHAKE_ANIMATION_KEY, animationCoroutine);
    }
    public void StopShakeAnimation()
    {
        StopAnimation(SHAKE_ANIMATION_KEY);
    }

}
