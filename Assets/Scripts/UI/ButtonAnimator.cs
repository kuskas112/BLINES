using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonAnimator : BasicObjectAnimator
{
    public const string GLOW_TOGGLE_ANIMATION_KEY = "glowToggle";
    private SpellButton spellButton; 

    [Header("Glow Animation Settings")]
    public float startGlow = 1f;
    public float maxGlow = 1.5f;
    public float duration = 0.2f;

    private void Awake()
    {
        spellButton = GetComponent<SpellButton>();
    }

    private void Start()
    {
        spellButton.button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        if(BattleManager.Instance.IsPlayerTurn())
        {
            StartGlowToggleAnimation(maxGlow, duration);
        }
    }

    public IEnumerator GlowToggleAnimation(float maxGlow = 0.5f, float duration = 1f)
    {
        float time = 0f;
        //float startGlow = 1f;
        float invDuration = 1 / duration;
        Color baseColor = spellButton.OutlineColor;
        while (time < duration / 2)
        {
            time += Time.deltaTime;
            float t = time * invDuration;
            float smoothT = Mathf.SmoothStep(startGlow, maxGlow, t);
            spellButton.OutlineColor = baseColor * smoothT;
            yield return null;
        }

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time * invDuration;
            float smoothT = Mathf.SmoothStep(maxGlow, startGlow, t);
            spellButton.OutlineColor = baseColor * smoothT;
            yield return null;
        }

        spellButton.OutlineColor = baseColor; // Убедиться, что цвет сброшен в исходное состояние
        StopGlowToggleAnimation();
    }

    public void StartGlowToggleAnimation(float maxGlow = 0.5f, float duration = 1f)
    {
        Coroutine animationCoroutine = StartCoroutine(GlowToggleAnimation(maxGlow, duration));
        StartAnimation(GLOW_TOGGLE_ANIMATION_KEY, animationCoroutine);
    }
    public void StopGlowToggleAnimation()
    {
        StopAnimation(GLOW_TOGGLE_ANIMATION_KEY);
    }
}
