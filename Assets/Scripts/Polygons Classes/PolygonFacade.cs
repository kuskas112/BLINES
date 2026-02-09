using UnityEngine;

// Класс, аггрегирующий все компоненты объекта полигона
public class PolygonFacade
{
    public Polygon polygon;
    public PolygonAnimator polygonAnimator;
    public Mover mover;
    public PolygonMaterialSetter materialSetter;

    public PolygonFacade(GameObject polygonObject)
    {
        polygon = polygonObject.GetComponent<Polygon>();
        polygonAnimator = polygonObject.GetComponent<PolygonAnimator>();
        mover = polygonObject.gameObject.GetComponent<Mover>();
        materialSetter = polygonObject.GetComponent<PolygonMaterialSetter>();
    }
}
