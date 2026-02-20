using System.Collections.Generic;
using UnityEngine;

public class DefaultRarenessSpellBundler : ISpellButtonPrefabBundler
{

    public List<SpellButtonFacade> GetAmountOfPrefabs(int amount)
    {
        List<SpellButtonFacade> result = new();
        SpellButtonPrefabSelector ps = SpellButtonPrefabSelector.Instance;
        List<Spell> spellList = new(ps.loadedSpells.Values);
        List<Spell> resultSpells = new();

        // Получить только Default-rare спеллы
        foreach(var spell in spellList)
        {
            if(spell.Rareness == SpellRareness.Default)
            {
                resultSpells.Add(spell);
            }
        }

        // Получить рандомные amount спеллов среди Default-rare спеллов
        foreach (var spell in GetRandomSpellValues(resultSpells, amount))
        {
            // Добавить в результат префаб связанный со спеллом
            result.Add(ps.GetPrefabBySpell(spell));
        }

        // На выходе amount рандомных спеллов 
        // с редкостью Default
        return result;
    }

    private List<Spell> GetRandomSpellValues(List<Spell> list, int amount)
    {
        if (list.Count < amount)
        {
            throw new System.Exception("Not enough spells in list");
        }
        List<Spell> result = new();
        for(int i = 0; i < amount; i++)
        {
            int randInd = Random.Range(0, list.Count);
            result.Add(list[randInd]);
            list.RemoveAt(randInd);
        }
        
        return result;
    }

}
