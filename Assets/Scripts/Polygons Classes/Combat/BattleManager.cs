using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private static BattleManager _instance;
    public static BattleManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Object.FindFirstObjectByType<BattleManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("BattleManager");
                    _instance = go.AddComponent<BattleManager>();
                }
            }
            return _instance;
        }
    }
    public BattleContext battleContext;
    public Fighter Player;
    public Fighter Enemy;
    void Awake()
    {
        battleContext = new BattleContext(
            Player,
            Enemy
        );
        for (int i = 0; i < 4; i++)
        {
            Player.spells.Add(
                new BasicAttack
                {
                    Rareness = (SpellRareness)(i + 1)
                }
            );
        }
    }

    void Start()
    {
        
    }
}
