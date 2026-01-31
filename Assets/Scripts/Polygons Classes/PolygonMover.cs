using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PolygonMover : MonoBehaviour
{
    public Polygon polygon;
    [HideInInspector]
    public bool IsMoving = false;
    public AnimationCurve MoveCurve = null;
    private AnimationCurve LinearCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [Header("Patrol Settings")]
    public bool patrolEnabled = false;
    public float patrolPointDuration = 1.5f;
    public enum PatrolCurveType
    {
        Linear,
        EaseOut
    }
    public PatrolCurveType patrolCurveType = PatrolCurveType.Linear;
    public Vector2[] patrolPoints;
    private Coroutine patrolCoroutine = null;

    private void Awake()
    {
        polygon ??= GetComponent<Polygon>();    
    }

    private void Start()
    {
        patrolPoints ??= new Vector2[]
        {
            new(-4f, 0f),
            new(4f, 0f),
            new(4f, 4f),
            new(-4f, 4f)
        };

        if (patrolEnabled) StartPatrol(patrolPointDuration);

        if(MoveCurve == null)
        {
            InitializeCurve();
        }
    }

    private void OnValidate()
    {
        if(Application.isPlaying) Start();
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

    // ==================== ПУБЛИЧНЫЕ МЕТОДЫ ====================

    public void StartPatrol(float pointDuration = 3f)
    {
        AnimationCurve curve = patrolCurveType == PatrolCurveType.EaseOut ? MoveCurve : LinearCurve;
        if (patrolCoroutine != null)
        {
            StopPatrol();
        }
        if (patrolEnabled && patrolPoints.Length > 1)
        {
            patrolCoroutine = StartCoroutine(PatrolCoroutine(pointDuration, curve));
        }
    }
    public void StopPatrol()
    {
        if (patrolCoroutine != null)
        {
            StopCoroutine(patrolCoroutine);
            patrolCoroutine = null;
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

    // ==================== КОРУТИНЫ ====================

    // Патрулирование между точками
    // Возвращается в начало после достижения последней точки
    // По умолчанию используется линейная кривая
    private IEnumerator PatrolCoroutine(float pointDuration, AnimationCurve curve = null)
    {
        curve ??= LinearCurve;
        int currentPointIndex = 0;
        while (patrolEnabled)
        {
            Vector2 targetPosition = patrolPoints[currentPointIndex];
            yield return MoveCoroutine(targetPosition, pointDuration, curve);
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }

    // Перемещение к целевой позиции с заданной кривой
    // По умолчанию используется EaseOut кривая
    private IEnumerator MoveCoroutine(Vector2 targetPosition, float duration, AnimationCurve curve = null)
    {
        IsMoving = true;
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
