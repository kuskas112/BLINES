using UnityEngine;

public class Modifier
{
    public Modifier(ModifierType type, ModifierTarget target, float value)
    {
        Type = type;
        Target = target;
        Value = value;
    }
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
