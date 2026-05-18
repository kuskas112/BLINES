using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HealthChangeEffect : MonoBehaviour
{

    public enum EffectDirection
    {
        Up,
        Down
    }

    [SerializeField] private Mover mover;
    [SerializeField] private Text text;
    private float startAngle = 30;
    private float endAngle = 150;

    public float StartRadius = 0.5f;
    public float EndRadius = 0.8f;
    public float Duratuion = 1f;
    public EffectDirection Direction = EffectDirection.Up;

    private void Awake()
    {
        if(mover    == null)    mover = GetComponent<Mover>();
        if(text == null)         text = GetComponent<Text>();
    }

    private void InitAnglesWithDirection()
    {
        // Если эффект будет вылетать вверх в рандомный угол
        if (Direction == EffectDirection.Up)
        {
            startAngle = 30;
            endAngle = 150;
        }
        else if (Direction == EffectDirection.Down)
        {
            startAngle = -30;
            endAngle = -150;
        }
    }

    public IEnumerator EffectCoroutine(float healthDiff)
    {
        string sign = healthDiff > 0 ? "+" : "";
        Color color = healthDiff > 0 ? Color.green : Color.red;
        string outText = $"{sign}{healthDiff}";
        text.text = outText;
        text.color = color;

        InitAnglesWithDirection();

        float angleDeg = Random.Range(startAngle, endAngle);
        float angleRad = angleDeg * Mathf.Deg2Rad;

        float distance = Random.Range(StartRadius, EndRadius);
        float x = Mathf.Cos(angleRad) * distance;
        float y = Mathf.Sin(angleRad) * distance;

        Vector2 target = transform.position;
        target += new Vector2(x, y);

        yield return mover.Move(target, Duratuion);
        text.color = Color.clear;
        Destroy(gameObject);
    }
}
