using UnityEngine;
using UnityEngine.UI;

public class FighterHPText : MonoBehaviour
{
    public Fighter fighter;
    private Text text;
    void Awake()
    {
        text = GetComponent<Text>();
        text.text = fighter.Health.ToString();
        fighter.onHealthChanged.AddListener(OnHealthChanged);
    }

    void OnHealthChanged(float health)
    {
        text.text = health.ToString();
    }


}
