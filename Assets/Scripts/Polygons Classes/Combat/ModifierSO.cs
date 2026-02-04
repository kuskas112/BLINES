using UnityEngine;


[CreateAssetMenu(fileName = "NewModifier", menuName = "BLINES/Modifiers/Advanced Modifier")]
public class ModifierSO : ScriptableObject
{
    public ModifierType Type;
    public ModifierTarget Target;
    public float Value;
    public float Apply(float baseValue)
    {
        return Type switch
        {
            ModifierType.Add => baseValue + Value,
            ModifierType.Mul => baseValue * Value,
            ModifierType.Set => Value,
            _ => baseValue,
        };
    }
}

public enum ModifierType
{
    Add,
    Mul,
    Set
}

public enum ModifierTarget
{
    AttackDamage,
    SelfHealth,
    TargetHealth,
}
