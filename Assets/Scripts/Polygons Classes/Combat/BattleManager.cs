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
        Modifier add10AttackDamageModifier = new Modifier(
            ModifierType.Add,
            ModifierTarget.AttackDamage,
            20
        );

        Player.spells[0].Modifiers.Add(add10AttackDamageModifier);
        // basic attack on enemy
        Player.spells[0].Cast(battleContext);
    }
}
