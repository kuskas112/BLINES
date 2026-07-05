using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextAnimator : BasicObjectAnimator
{
    const string CHANGE_COLOR_ANIMATION_KEY = "changeColor";
    public Text text;

    private void Awake()
    {
        if (text == null)
        {
            text = GetComponent<Text>();
        }
    }


    public Coroutine StartChangeColorAnimation(Color targetColor, float duration = 1f)
    {
        Coroutine animationCoroutine = StartCoroutine(ChangeColorAnimation(targetColor, duration));
        StartAnimation(CHANGE_COLOR_ANIMATION_KEY, animationCoroutine);
        return animationCoroutine;
    }
    public IEnumerator ChangeColorAnimation(Color targetColor, float duration = 1f)
    {
        float time = 0f;
        float invDuration = 1 / duration;
        Color startColor = text.color;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time * invDuration;
            Color newColor = Color.Lerp(startColor, targetColor, t);
            text.color = newColor;
            yield return null;
        }
        text.color = targetColor; // Убедиться, что цвет сброшен в целевое состояние
        StopChangeColorAnimation();
    }
    public void StopChangeColorAnimation()
    {
        StopAnimation(CHANGE_COLOR_ANIMATION_KEY);
    }

}
