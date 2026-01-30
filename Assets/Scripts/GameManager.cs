using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private PolygonMover mover;
    private GameObject polygonObject;
    void Start()
    {
        polygonObject = GameObject.Find("Polygon");
        mover = polygonObject.GetComponent<PolygonMover>();
    }

    // Update is called once per frame
    void Update()
    {
        if (polygonObject.transform.position.x < 0 && mover.IsMoving == false)
        {
            mover.MovePolygonEaseOut(new Vector2(3f, 3f), 0.75f);
        }
        else if (polygonObject.transform.position.x >= 0 && mover.IsMoving == false)
        {
            mover.MovePolygonLinear(new Vector2(-3f, -3f), 0.75f);
        }
    }
}
