using UnityEngine;

public class DoorFacade : ButtonFacade
{
    [HideInInspector] public DoorBehaviour doorBehaviour;
    protected override void Awake()
    {
        base.Awake();
        doorBehaviour = GetComponent<DoorBehaviour>();
    }

}
