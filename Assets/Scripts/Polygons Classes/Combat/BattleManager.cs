using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
                    Rareness = (SpellRareness)i,
                    Modifiers = new List<Modifier>
                    {
                        new Modifier
                        (
                            ModifierType.Add,
                            ModifierTarget.AttackDamage,
                            10f * (i + 1)
                        )
                    }
                }
            );

            Enemy.spells.Add(
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

    public bool IsPlayerTurn()
    {
        return battleContext.IsPlayerTurn;
    }
    public bool IsEnemyTurn()
    {
        return !battleContext.IsPlayerTurn;
    }

    public void NextTurn()
    {
        battleContext.NextTurn();
    }

    public Spell GetPlayerSpell(int index)
    {
        if (index >= 0 && index < battleContext.Player.spells.Count)
        {
            return battleContext.Player.spells[index];
        }
        Debug.LogWarning("Invalid player spell index: " + index + ". Player has " + battleContext.Player.spells.Count + " spells.");
        return null;
    }

    public Spell GetEnemySpell(int index)
    {
        if (index >= 0 && index < battleContext.Enemy.spells.Count)
        {
            return battleContext.Enemy.spells[index];
        }
        Debug.LogWarning("Invalid enemy spell index: " + index + ". Enemy has " + battleContext.Enemy.spells.Count + " spells.");
        return null;
    }

    public void CastPlayerSpell(int index)
    {
        if (!IsPlayerTurn())
        {
            Debug.LogWarning("It's not player's turn!");
            return;
        }
        Spell spell = GetPlayerSpell(index);
        if (spell != null)
        {
            spell.Cast(battleContext);
            NextTurn();
            if (IsEnemyHaveSpells())
            {
                CastEnemySpell(Random.Range(0, battleContext.Enemy.spells.Count));
            }
            else
            {
                // Если у врага нет заклинаний, он просто пропускает ход
                NextTurn();
            }
        }
    }

    public bool IsEnemyHaveSpells()
    {
        return battleContext.Enemy.spells.Count > 0;
    }

    public bool IsPlayerHaveSpells()
    {
        return battleContext.Player.spells.Count > 0;
    }

    public void CastEnemySpell(int index)
    {
        if (!IsEnemyTurn())
        {
            Debug.LogWarning("It's not enemy's turn!");
            return;
        }
        Spell spell = GetEnemySpell(index);
        if (spell != null)
        {
            spell.Cast(battleContext);
            NextTurn();
        }
    }
}
