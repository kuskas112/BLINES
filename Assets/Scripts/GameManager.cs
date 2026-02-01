using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject polygonObject;
    private Polygon polygon;
    private PolygonAnimator polygonAnimator;
    private MaterialSetter materialSetter;
    void Start()
    {
        polygonObject = GameObject.Find("MorphTesterPolygon");
        polygon = polygonObject?.GetComponent<Polygon>();
        polygonAnimator = polygonObject?.GetComponent<PolygonAnimator>();
        materialSetter = polygonObject?.GetComponent<MaterialSetter>();
    }

    // Update is called once per frame
    void Update()
    {
        int angleLimit = 8;
        if(polygonObject == null) return;
        if(polygon.Angles < angleLimit && polygonAnimator.IsAnimationRunning(PolygonAnimator.MORPH_ANIMATION_KEY) == false)
        {
            float time = angleLimit - polygon.Angles; 
            polygonAnimator.StartMorphAnimation(angleLimit, time);
            materialSetter.StartLerpEdgeNeonColor(Color.green, time);
        }
        else if(polygon.Angles >= angleLimit && polygonAnimator.IsAnimationRunning(PolygonAnimator.MORPH_ANIMATION_KEY) == false)
        {
            float time = angleLimit - 3; 
            polygonAnimator.StartMorphAnimation(3, time);
            materialSetter.StartLerpEdgeNeonColor(Color.black, time);
        }
    }
}
