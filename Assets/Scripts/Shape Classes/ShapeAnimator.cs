using UnityEngine;

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

    private void Start()
    {
        if (autoStartAnimation)
        {
            StartDefaultAnimations();
        }
    }

    private void StartDefaultAnimations()
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
}