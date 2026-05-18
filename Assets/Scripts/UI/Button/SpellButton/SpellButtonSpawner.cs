using UnityEngine;

public class SpellButtonSpawner : UIElementSpawner<SpellButtonFacade>
{
    public override SpellButtonFacade Spawn(Vector3 position, Quaternion rotation)
    {
        SpellButtonFacade facade = base.Spawn(position, rotation);
        facade.behaviour.MoveObjectInsideButton();
        return facade;
    }
}