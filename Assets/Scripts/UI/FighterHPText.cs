using UnityEngine;
using UnityEngine.UI;

public class FighterHPText : MonoBehaviour
{
    public Fighter fighter;
    private Text text;
    public HealthChangeEffect hpChangeEffect;
    void Awake()
    {
        text = GetComponent<Text>();
        text.text = fighter.Health.ToString();
        fighter.onHealthChanged.AddListener(OnHealthChanged);
        if(hpChangeEffect == null)
        {
            hpChangeEffect = GetComponentInChildren<HealthChangeEffect>();
        }
    }

    void OnHealthChanged(float health, float healthDiff)
    {
        text.text = health.ToString();
        if(hpChangeEffect != null) StartCoroutine(hpChangeEffect.EffectCoroutine(healthDiff));
    }


}
