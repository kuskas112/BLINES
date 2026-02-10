using UnityEngine;

// Класс, аггрегирующий все компоненты объекта полигона
public class PolygonFacade : MonoBehaviour
{
    [HideInInspector] public Polygon polygon;
    [HideInInspector] public PolygonAnimator animator;
    [HideInInspector] public Mover mover;
    [HideInInspector] public PolygonMaterialSetter materialSetter;

    private void Awake()
    {
        polygon         = GetComponent<Polygon>();
        animator = GetComponent<PolygonAnimator>();
        mover           = GetComponent<Mover>();
        materialSetter  = GetComponent<PolygonMaterialSetter>();
    }
}
