using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fighter : MonoBehaviour
{
    public List<Spell> spells = new();
    public PolygonFacade polygonFacade;
    public float Health = 100f;
    public GameObject PolygonObject;

    void Awake()
    {
        polygonFacade = new PolygonFacade(PolygonObject);
        // У всех есть базовая атака
        spells.Add(new BasicAttack());
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
