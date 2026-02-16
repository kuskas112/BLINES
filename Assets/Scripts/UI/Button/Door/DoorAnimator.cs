using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DoorAnimator : ButtonAnimator
{
    public const string DOOR_DISAPPEAR_ANIMATION_KEY = "DoorDisappear";
    [Header("Door Animation Settings")]
    public float doorDisappearDuration = 1f;

    public override void StartOnClickAnimation()
    {
        base.StartOnClickAnimation();
        StartDoorDisappearAnimation(doorDisappearDuration);
    }
    public IEnumerator DoorDisappearAnimation(float duration = 1f)
    {
        Material mat = buttonMaterialSetter.EdgeMaterial;
        DoorMaterialSetter matSetter = buttonMaterialSetter as DoorMaterialSetter;
        float time = 0f;
        float invDuration = 1 / duration;
        float initialOutlineWidth = matSetter.OutlineWidth;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time * invDuration;
            float smoothT = Mathf.SmoothStep(initialOutlineWidth, -0.1f, t);
            matSetter.OutlineWidth = smoothT;
            yield return null;
        }
        matSetter.OutlineWidth = 0f;
        matSetter.PulseSpeed = 0f; // стоп анимации


        StopDoorDisappearAnimation();
    }
    public void StartDoorDisappearAnimation(float duration = 1f)
    {
        StartAnimation(DOOR_DISAPPEAR_ANIMATION_KEY, StartCoroutine(DoorDisappearAnimation(duration)));
    }
    public void StopDoorDisappearAnimation()
    {
        StopAnimation(DOOR_DISAPPEAR_ANIMATION_KEY);
    }
}
