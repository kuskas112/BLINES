using UnityEngine;

public class SpellButtonPrefabSelector : PrefabSelector<SpellButtonFacade>
{
    private static SpellButtonPrefabSelector _instance;
    public static SpellButtonPrefabSelector Instance
        => _instance ??= new SpellButtonPrefabSelector();
    private SpellButtonPrefabSelector()
    {
        Init();
    }

    protected override SpellButtonFacade[] Init()
    {
        SpellButtonFacade[] prefabs = base.Init();
        foreach (var prefab in prefabs)
        {
            Spell spell = prefab.behaviour.GetSpell();
            Debug.Log("LALALA " + spell.Name);
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
