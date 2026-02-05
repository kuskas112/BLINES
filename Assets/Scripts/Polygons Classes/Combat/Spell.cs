using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SpellType {Active, Passive, Consumable}
public class Spell
{
    public string Name;
    public string Description;
    public SpellType Type;
    public List<Modifier> Modifiers = new();

    public Spell()
    {
        Name = "Spell";
        Description = "Default Description";
        Type = SpellType.Passive;
    }

    public virtual void Cast(BattleContext context)
    {
        #if UNITY_EDITOR
        Debug.Log("Casted " + Name);
        #endif
    }    
}
