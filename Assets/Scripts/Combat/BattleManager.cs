using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
        if (IsPlayerTurn()) {
            Player.polygonFacade.animator.StopPulseAnimation();
            Enemy.polygonFacade.animator.StartPulseAnimation();
        }
        else
        {
            Enemy.polygonFacade.animator.StopPulseAnimation();
            Player.polygonFacade.animator.StartPulseAnimation();
        }
        battleContext.NextTurn();
    }

    private void AddSpellTo(Fighter fighter, Spell spell)
    {
        if (IsSpellIn(fighter, spell) == false)
        {
            fighter.spells.Add(spell);
        }
    }

    public void AddSpellToPlayer(Spell spell)
    {
        AddSpellTo(Player, spell);
    }

    public void AddSpellToEnemy(Spell spell)
    {
        AddSpellTo(Enemy, spell);
    }

    private bool IsSpellIn(Fighter fighter, Spell spell)
    {
        if (fighter == null || spell == null)
        {
            Debug.LogError("Invalid fighter or spell in IsSpellIn");
            return false;
        }
        foreach(Spell plSpell in fighter.spells)
        {
            if (plSpell != null && plSpell == spell)
            {
                return true;
            }
            if(plSpell.Name == spell.Name)
            {
                string fighterName = fighter == Player ? "Player" : "Enemy";
                Debug.LogError(fighterName + " already has another spell with name '" + spell.Name + "'");
                return false;
            }
        }
        return false;
    }

    private void Cast(Spell spell)
    {
        spell.Cast(battleContext);
    }

    public void CastPlayerSpell(Spell spell)
    {
        if (!IsPlayerTurn())
        {
            Debug.LogWarning("It's not player's turn!");
            return;
        }
        if (spell != null && IsSpellIn(Player, spell))
        {
            Cast(spell);
            Debug.Log("Player casted " + spell.Name);
            NextTurn();
        }
        else
        {
            Debug.LogError("Invalid Spell");
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

    public void CastEnemySpell(Spell spell)
    {
        if (!IsEnemyTurn())
        {
            Debug.LogWarning("It's not enemy's turn!");
            return;
        }
        if (spell != null && IsSpellIn(Enemy, spell))
        {
            Cast(spell);
            NextTurn();
        }
    }
}
