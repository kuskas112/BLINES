using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class SpellButtonPlacementManager : MonoBehaviour
{
    private Fighter fighter;
    public List<Vector2> positions;
    public FighterType type;
    private SpellButtonSpawner spawner;
    private List<SpellButtonFacade> spawnedInstances = new();
    private void Start()
    {
        if(type == FighterType.Player)
        {
            fighter = BattleManager.Instance.Player;
        }
        else if (type == FighterType.Enemy)
        {
            fighter = BattleManager.Instance.Enemy;
        }
        spawner = FindAnyObjectByType<SpellButtonSpawner>();


        if(type == FighterType.Enemy)
        {
            BattleManager.Instance.AddSpellToEnemy(new BasicAttack());
            BattleManager.Instance.AddSpellToEnemy(new Block());
            BattleManager.Instance.AddSpellToEnemy(new DownsPizza());
        }
        else
        {
            BattleManager.Instance.AddSpellToPlayer(new BasicAttack());
            BattleManager.Instance.AddSpellToPlayer(new Block());
            BattleManager.Instance.AddSpellToPlayer(new DownsPizza());
        }


        SpawnButtonsOnPositions();

    }

    public void DestroySpawnedButtons()
    {
        foreach (var inst in spawnedInstances)
        {
            Destroy(inst.gameObject);
        }
    }

    public void SpawnButtonsOnPositions()
    {
        DestroySpawnedButtons();
        for(int i = 0; i < fighter.spells.Count; i++) {
            Vector3 pos = positions[i];
            Spell spell = fighter.spells[i];
            SpellButtonFacade facade = SpellButtonPrefabSelector.Instance.GetPrefabBySpell(spell);
            spawner.SetPrefab(facade);
            var inst = spawner.Spawn(pos, Quaternion.identity);
            inst.behaviour.SetSpell(spell);
            spawnedInstances.Add(inst);
            Debug.Log("Spawned button for " + spell.Name);
        }
    }
}
