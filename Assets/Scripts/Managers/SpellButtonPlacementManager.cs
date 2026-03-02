using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class SpellButtonPlacementManager : MonoBehaviour
{
    public List<Vector2> positions;
    public FighterType type;
    public float SlideDuration = 2f;
    public List<SpellButtonFacade> spawnedInstances = new();

    private Fighter fighter;
    private SpellButtonSpawner spawner;
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

    public void SlideButtonsOnPositions()
    {
        DestroySpawnedButtons();
        float startPosOffset = -10f; // Кнопки игрока прилетают снизу
        if (type == FighterType.Enemy) startPosOffset *= -1; // а врага сверху
        for (int i = 0; i < fighter.spells.Count; i++)
        {
            Vector3 endPos = positions[i];
            Vector3 pos = positions[i];
            pos.y = pos.y + startPosOffset;

            Spell spell = fighter.spells[i];

            SpellButtonFacade facade = SpellButtonPrefabSelector.Instance.GetPrefabBySpell(spell);
            spawner.SetPrefab(facade);

            var inst = spawner.Spawn(pos, Quaternion.identity);

            inst.behaviour.SetSpell(spell);
            spawnedInstances.Add(inst);

            inst.mover.MoveEaseOut(endPos, SlideDuration);
        }
    }
}
