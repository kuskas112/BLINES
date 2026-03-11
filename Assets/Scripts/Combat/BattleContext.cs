using UnityEngine;
using UnityEngine.Events;

public class BattleContext
{
    public BattleContext(Fighter player, Fighter enemy)
    {
        Player = player;
        PlayerButtonsPlacer = GameObject.Find("PlayerButtons").GetComponent<SpellButtonPlacementManager>();

        Enemy = enemy;
        EnemyButtonsPlacer = GameObject.Find("EnemyButtons").GetComponent<SpellButtonPlacementManager>();
    }
    public UnityEvent<int> OnTurnSwitched = new();

    public Fighter Player;
    public Fighter Enemy;

    public SpellButtonPlacementManager PlayerButtonsPlacer;
    public SpellButtonPlacementManager EnemyButtonsPlacer;

    public bool IsPlayerTurn = true;
    public int Round = 1;

    public void NextTurn()
    {
        Round++;
        IsPlayerTurn = !IsPlayerTurn;
        OnTurnSwitched.Invoke(Round);
    }
}
