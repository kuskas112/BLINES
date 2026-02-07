using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class Fighter1fEvent : UnityEvent<float> {}
public class Fighter : MonoBehaviour
{
    public Fighter1fEvent onHealthChanged = new();
    public List<Spell> spells = new();
    public PolygonFacade polygonFacade;
    private float _health = 100f;
    public float Health
    {
        get { return _health; }
        set
        {
            _health = value;
            onHealthChanged.Invoke(_health);
        }
    }
    public GameObject PolygonObject;

    void Awake()
    {
        polygonFacade = new PolygonFacade(PolygonObject);
    }

    public Polygon GetPolygon()
    {
        return polygonFacade.polygon;
    }

    public void TakeDamage(float damage)
    {
        #if UNITY_EDITOR
        Debug.Log("Damage taken: " + damage);
        #endif
        
        Health -= damage;
        polygonFacade.polygonAnimator.StartBounceAnimation(0.2f);
        if (Health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        #if UNITY_EDITOR
        Debug.Log("Die");
        #endif
    }



}
