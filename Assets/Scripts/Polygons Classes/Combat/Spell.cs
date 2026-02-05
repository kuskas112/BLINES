using UnityEngine;
using System;

public enum SpellType {Active, Passive, Consumable}
public class Spell
{
    public string Name;
    public string Description;
    public SpellType Type;
    public ModifierSO[] Modifiers = Array.Empty<ModifierSO>();

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
