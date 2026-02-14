using UnityEngine;

public class BattleContext
{
    public BattleContext(Fighter player, Fighter enemy)
    {
        Player = player;
        Enemy = enemy;
    }
    public Fighter Player;
    public Fighter Enemy;
    public bool IsPlayerTurn = true;
    public int Round = 1;

    public void NextTurn()
    {
        Round++;
        IsPlayerTurn = !IsPlayerTurn;
    }
}
