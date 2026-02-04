using UnityEngine;

public enum SpellType {Active, Passive, Consumable}
public class Spell : MonoBehaviour
{
    public string Name = "Spell";
    public string Description = "Default Description";
    public SpellType Type = SpellType.Passive;
    public ModifierSO[] Modifiers;

    public virtual void Cast()
    {
        Debug.Log("Casted " + Name);
    }    
}
