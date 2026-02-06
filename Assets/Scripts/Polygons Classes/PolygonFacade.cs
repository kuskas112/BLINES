using UnityEngine;

// Класс, аггрегирующий все компоненты объекта полигона
public class PolygonFacade
{
    public Polygon polygon;
    public PolygonAnimator polygonAnimator;
    public Mover mover;
    public MaterialSetter materialSetter;

    public PolygonFacade(GameObject polygonObject)
    {
        polygon = polygonObject.GetComponent<Polygon>();
        polygonAnimator = polygonObject.GetComponent<PolygonAnimator>();
        mover = polygonObject.gameObject.GetComponent<Mover>();
        materialSetter = polygonObject.GetComponent<MaterialSetter>();
    }
}
