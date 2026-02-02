using UnityEngine;
using UnityEngine.U2D;
using System.Reflection;
using System;

public class Polygon : MonoBehaviour
{
    [SerializeField] protected int _angles = 4;
    [SerializeField] protected float _radius = 1f;
    [SerializeField] protected Corner cornerMode = Corner.Stretched;
    private static Action<Spline, int, int> setCornerModeDelegate;


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

    [SerializeField] protected SpriteShapeController shController;
    [SerializeField] protected SpriteShapeRenderer shRenderer;
    public const int MaxAngles = 20;

    protected virtual void Awake()
    {
        shController ??= GetComponent<SpriteShapeController>();
        shRenderer ??= GetComponent<SpriteShapeRenderer>();
        InitializeDelegate();
    }

    protected virtual void Start()
    {
        UpdatePolygonShape();
    }

    protected static void InitializeDelegate()
    {
        var method = typeof(Spline).GetMethod("SetCornerMode",
            BindingFlags.NonPublic | BindingFlags.Instance);
        
        if (method != null)
        {
            setCornerModeDelegate = (Action<Spline, int, int>)
                Delegate.CreateDelegate(typeof(Action<Spline, int, int>), method);
            
        }
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
            
            shController.spline.SetTangentMode(i, ShapeTangentMode.Linear);
            shController.spline.SetCorner(i, true);
            setCornerModeDelegate?.Invoke(shController.spline, i, (int)cornerMode);
        }
        
        shController.spline.isOpenEnded = false;
        shController.RefreshSpriteShape();
    }

    protected void OnValidate()
    {
        if(shController != null && shRenderer != null) Angles = _angles; // Триггер обновления при изменении в инспекторе
    }
}