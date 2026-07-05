using UnityEngine;
using UnityEngine.UI;

public class FighterHPText : MonoBehaviour
{
    public Fighter fighter;
    
    private TextAnimator animator;
    private Text text;
    private HealthChangeEffectSpawner hpEffectSpawner;
    void Awake()
    {
        text = GetComponent<Text>();
        text.text = string.Empty;
        text.color = new Color(0, 0, 0, 0);

        if (animator == null) 
        {
            animator = GetComponent<TextAnimator>();
        }

        fighter.onHealthChanged.AddListener(OnHealthChanged);
        if(hpEffectSpawner == null)
        {
            hpEffectSpawner = FindAnyObjectByType<HealthChangeEffectSpawner>();
        }

        BattleManager.Instance.OnBattleStart.AddListener(() =>
        {
            text.text = fighter.Health.ToString();
            Color newColor = fighter.type == FighterType.Player ? Color.green : Color.red;
            animator.StartChangeColorAnimation(newColor);
        });
    }

    void OnHealthChanged(float health, float healthDiff)
    {
        text.text = health.ToString();

        var hpChangeEffect = hpEffectSpawner.Spawn(fighter.PolygonObject.transform.position, transform.rotation);

        if (fighter.type == FighterType.Player)
        {
            hpChangeEffect.Direction = HealthChangeEffect.EffectDirection.Up;
        }
        else
        {
            hpChangeEffect.Direction = HealthChangeEffect.EffectDirection.Down;
        }

        StartCoroutine(hpChangeEffect.EffectCoroutine(healthDiff));
    }


}
