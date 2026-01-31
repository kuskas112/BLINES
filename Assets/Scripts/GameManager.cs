using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject polygonObject;
    private Polygon polygon;
    private PolygonAnimator polygonAnimator;
    void Start()
    {
        polygonObject = GameObject.Find("MorphTesterPolygon");
        polygon = polygonObject.GetComponent<Polygon>();
        polygonAnimator = polygonObject.GetComponent<PolygonAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(polygon.Angles < 12 && polygonAnimator.IsAnimationRunning(PolygonAnimator.MORPH_ANIMATION_KEY) == false)
        {
            polygonAnimator.StartMorphAnimation(12, 8f);
        }
        else if(polygon.Angles >= 12 && polygonAnimator.IsAnimationRunning(PolygonAnimator.MORPH_ANIMATION_KEY) == false)
        {
            polygonAnimator.StartMorphAnimation(3, 8f);
        }
    }
}
