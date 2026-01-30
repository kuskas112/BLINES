using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PolygonMover : MonoBehaviour
{
    public Polygon polygon;
    public bool IsMoving = false;
    public AnimationCurve MoveCurve = null;
    private AnimationCurve LinearCurve = AnimationCurve.Linear(0, 0, 1, 1);

    private void Awake()
    {
        polygon ??= GetComponent<Polygon>();    
    }

    private void Start()
    {
        if(MoveCurve == null)
        {
            InitializeCurve();
        }
    }

    // Подобранная вручную кривая,
    // оно того не стоит, лучше в инспекторе нарисовать
    // ну оставлю на всякий случай
    private void InitializeCurve()
    {
        // Создаем новую кривую
        MoveCurve = new AnimationCurve();
        Keyframe[] keys =
        {
            new Keyframe(0, 0, 0, 0),
            new Keyframe(0.111f, 0.024f, 0.5f, 0.5f),
            new Keyframe(0.361f, 0.769f, 1, 1),
            new Keyframe(1, 1, 0, 0)
        };
        // Добавляем ключи
        foreach (Keyframe key in keys)
        {
            MoveCurve.AddKey(key);
        }
    }

    public void MovePolygon(Vector2 targetPosition, float duration, AnimationCurve curve = null)
    {
        if (!IsMoving)
        {
            StartCoroutine(MoveCoroutine(targetPosition, duration, curve));
        }
    }

    public void MovePolygonEaseOut(Vector2 targetPosition, float duration)
    {
        MovePolygon(targetPosition, duration, MoveCurve);
    }
    public void MovePolygonLinear(Vector2 targetPosition, float duration)
    {
        MovePolygon(targetPosition, duration, LinearCurve);
    }

    private IEnumerator MoveCoroutine(Vector2 targetPosition, float duration, AnimationCurve curve = null)
    {
        IsMoving = true;
        yield return new WaitForSeconds(1f);
        Vector2 startPosition = polygon.transform.position;
        float time = 0f;
        AnimationCurve usedCurve = curve ?? MoveCurve;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            
            float smoothT = usedCurve.Evaluate(t);
            polygon.transform.position = Vector2.Lerp(startPosition, targetPosition, smoothT);
            yield return null;
        }

        polygon.transform.position = targetPosition;
        IsMoving = false;
    }

}
