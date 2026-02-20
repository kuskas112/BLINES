using UnityEngine;
using System.Collections.Generic;

public class SpellButtonPrefabSelector : PrefabSelector<SpellButtonFacade>
{
    private static SpellButtonPrefabSelector _instance;
    public static SpellButtonPrefabSelector Instance
        => _instance ??= new SpellButtonPrefabSelector();
    private SpellButtonPrefabSelector()
    {
        Init();
    }
    public Dictionary<string, Spell> loadedSpells = new();

    protected override SpellButtonFacade[] Init()
    {
        SpellButtonFacade[] prefabs = base.Init();
        foreach (var prefab in prefabs)
        {
            prefab.Init();
            Spell spell = prefab.behaviour.GetSpell();
            loadedSpells.Add(spell.Name, spell);
        }
        return prefabs;
    }
    protected override string prefabPath => "Prefabs/SpellButtons/Variants";

    public SpellButtonFacade GetPrefabBySpell(Spell spell)
    {
        string name = spell.GetType().Name;
        return GetPrefabByName(name);
    }
}
