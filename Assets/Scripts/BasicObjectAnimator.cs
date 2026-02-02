using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicObjectAnimator : MonoBehaviour
// Появилась нужда в произвольных полигонах, не наследуемых от класса Polygon.
// К ним применимы не все анимации из PolygonAnimator.cs, поэтому выношу общие корутины сюда.
{
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
