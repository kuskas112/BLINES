using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private BattleContext battleContext;
    public Fighter Player;
    public Fighter Enemy;
    void Awake()
    {
        battleContext = new BattleContext(
            Player,
            Enemy
        );
    }

    void Start()
    {
        // basic attack on enemy
        Player.spells[0].Cast(battleContext);
    }
}
