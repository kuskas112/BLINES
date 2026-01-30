using UnityEngine;
using UnityEngine.U2D;

public class Polygon : MonoBehaviour
{
    [SerializeField] protected int _angles = 4;
    [SerializeField] protected float _radius = 1f;
    public int Angles
    {
        get => _angles;
        set 
        {
            _angles = Mathf.Clamp(value, 3, 20);
            UpdatePolygonShape();
        }
    }

    public float Radius
    {
        get => _radius;
        set 
        {
            _radius = Mathf.Max(0.1f, value);
            UpdatePolygonShape();
        }
    }

    protected SpriteShapeController shController;
    protected SpriteShapeRenderer shRenderer;
    public const int MaxAngles = 20;

    protected virtual void Awake()
    {
        shController ??= GetComponent<SpriteShapeController>();
        shRenderer ??= GetComponent<SpriteShapeRenderer>();
    }

    protected virtual void Start()
    {
        UpdatePolygonShape();
    }
   
    protected virtual void UpdatePolygonShape()
    {
        shController.spline.Clear();

        for (int i = 0; i < Angles; i++)
        {
            float angle = i * (360f / Angles);
            float x = Radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = Radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            
            shController.spline.InsertPointAt(i, new Vector3(x, y, 0));
            
            // Делаем углы острыми
            shController.spline.SetTangentMode(i, ShapeTangentMode.Linear);
        }
        
        shController.spline.isOpenEnded = false;
        shController.RefreshSpriteShape();
    }

    private void OnValidate()
    {
        if(shController != null && shRenderer != null) Angles = _angles; // Триггер обновления при изменении в инспекторе
    }
}