using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HealthChangeEffect : MonoBehaviour
{
    public Mover mover;
    public BasicObjectAnimator animator;
    private Text text;

    private void Awake()
    {
        if(animator == null) animator = GetComponent<BasicObjectAnimator>();
        if(mover    == null)    mover = GetComponent<Mover>();
        if(text == null)         text = GetComponent<Text>();
    }

    public IEnumerator EffectCoroutine(float healthDiff)
    {
        string sign = healthDiff > 0 ? "+" : "";
        Color color = healthDiff > 0 ? Color.green : Color.red;
        //string outText = SpellDescriptionBehaviour.ColorString($"{sign}{healthDiff}", color);
        string outText = $"{sign}{healthDiff}";
        text.text = outText;
        text.color = color;
        Vector2 start = transform.position;
        Vector2 target = transform.position;
        target.x += 1f;
        yield return mover.Move(target, 2f);
        text.color = Color.clear;
        transform.position = start;
    }
}
