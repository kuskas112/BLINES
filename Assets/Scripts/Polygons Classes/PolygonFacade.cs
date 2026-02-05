using UnityEngine;

// Класс, аггрегирующий все компоненты объекта полигона
public class PolygonFacade
{
    public Polygon polygon;
    public PolygonAnimator polygonAnimator;
    public PolygonMover polygonMover;
    public MaterialSetter materialSetter;

    public PolygonFacade(GameObject polygonObject)
    {
        polygon = polygonObject.GetComponent<Polygon>();
        polygonAnimator = polygonObject.GetComponent<PolygonAnimator>();
        polygonMover = polygonObject.gameObject.GetComponent<PolygonMover>();
        materialSetter = polygonObject.GetComponent<MaterialSetter>();
    }
}
