using UnityEngine;
using System.Collections.Generic;

// Интерфейс, позволяющий организовать поставку нескольких префабов для
// дверей (лутбоксов). Нельзя просто на рандом выдавать игроку любой спелл, выдачу
// нужно контроллировать, в зависимости от редкости или от длительности партии.
public interface ISpellButtonPrefabBundler
{
    //public List<SpellButtonFacade> GetPrefabs();
    public List<SpellButtonFacade> GetAmountOfPrefabs(int amount);
}
