using UnityEngine;
using UnityEngine.UI;

public class FighterHPText : MonoBehaviour
{
    public Fighter fighter;
    
    private Text text;
    private HealthChangeEffectSpawner hpEffectSpawner;
    void Awake()
    {
        text = GetComponent<Text>();
        text.text = fighter.Health.ToString();
        fighter.onHealthChanged.AddListener(OnHealthChanged);
        if(hpEffectSpawner == null)
        {
            hpEffectSpawner = FindAnyObjectByType<HealthChangeEffectSpawner>();
        }
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
