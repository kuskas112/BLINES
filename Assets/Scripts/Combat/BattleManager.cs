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

    private Queue<Coroutine> activeCastAnimations = new();

    void Awake()
    {
        battleContext = new BattleContext(
            Player,
            Enemy
        );

        battleContext.OnTurnSwitched.AddListener(NextTurn);

        Enemy.spells.Add(new BasicAttack());
        Enemy.spells.Add(new FibonacciAttack());
        Enemy.spells.Add(new Block());

        Player.spells.Add(new DownsPizza());
        Player.spells.Add(new SnowballAttack());
    }

    public void SpawnFighters(Vector2 playerPos, Vector2 enemyPos)
    {
        var spawner = FindAnyObjectByType<PolygonSpawner>();

        var playerInst = spawner.Spawn(new(0, -10), Quaternion.identity);
        Player.PolygonObject = playerInst.polygon.gameObject;
        Player.polygonFacade = playerInst;

        var enemyInst  = spawner.Spawn(new(0,  10), Quaternion.identity);
        Enemy.PolygonObject = enemyInst.polygon.gameObject;
        Enemy.polygonFacade = enemyInst;

        playerInst.mover.MoveEaseOut(playerPos, 1.5f);
        enemyInst.mover.Move(enemyPos, 1.5f);
    }

    private void DestroyFighter(Fighter fighter) 
    {
        Destroy(fighter.polygonFacade.polygon.gameObject);
        Destroy(fighter.gameObject);
    }

    public bool IsPlayerTurn()
    {
        return battleContext.IsPlayerTurn;
    }
    public bool IsEnemyTurn()
    {
        return !battleContext.IsPlayerTurn;
    }

    public void NextTurn(int round)
    {
        if (IsPlayerTurn()) {
            Enemy.polygonFacade.animator.StopPulseAnimation();
            Player.polygonFacade.animator.StartPulseAnimation();
        }
        else
        {
            Player.polygonFacade.animator.StopPulseAnimation();
            Enemy.polygonFacade.animator.StartPulseAnimation();
        }
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

    public void PlayCastAnimation(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
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
        }
    }
}
