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
    protected override string prefabPath => "Prefabs/SpellButtons/Variants";

    public SpellButtonFacade GetPrefabBySpell(Spell spell)
    {
        string name = spell.GetType().Name;
        return GetPrefabByName(name);
    }
}
