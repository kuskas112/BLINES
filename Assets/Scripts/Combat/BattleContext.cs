using UnityEngine;
using UnityEngine.Events;

public class BattleContext
{
    public BattleContext(Fighter player, Fighter enemy)
    {
        Player = player;
        Enemy = enemy;
    }
    public UnityEvent<int> OnTurnEnded = new();
    public UnityEvent<int> OnTurnStarted = new();

    public Fighter Player;
    public Fighter Enemy;
    public bool IsPlayerTurn = true;
    public int Round = 1;

    public void NextTurn()
    {
        OnTurnStarted.Invoke(Round);
        Round++;
        IsPlayerTurn = !IsPlayerTurn;
        OnTurnEnded.Invoke(Round);
    }
}
