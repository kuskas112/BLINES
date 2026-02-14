using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class Fighter1fEvent : UnityEvent<float> {}
public class Fighter : MonoBehaviour
{
    public Fighter1fEvent onHealthChanged = new();
    public Fighter1fEvent onDefenceChanged = new();
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
    
    private float _defence = 0f;
    public float Defence
    {
        get { return _defence; }
        set 
        {
            _defence = Mathf.Clamp(value, 0f, 100f);
            onDefenceChanged.Invoke(_defence);
        }
    }

    public GameObject PolygonObject;

    void Awake()
    {
        polygonFacade = PolygonObject.GetComponent<PolygonFacade>();
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
        damage = UseDefence(damage);
        Health -= damage;
        //TODO: Event system
        polygonFacade.animator.StartBounceAnimation(0.2f);
        if (Health <= 0)
        {
            Die();
        }
    }

    private float UseDefence(float incomingDamage)
    {
        float incDmg = incomingDamage * (1 - (Defence / 100f));
        #if UNITY_EDITOR
        Debug.Log("Damage reduced by defence: " + incDmg);
        #endif
        return incDmg;
    }

    public void TakeHeal(float heal)
    {
        #if UNITY_EDITOR
        Debug.Log("Heal taken: " + heal);
        #endif
        
        Health += heal;
        //TODO: Event system
        polygonFacade.animator.StartRotationChangeAnimation(GetPolygon().transform, 1, 360);
    }

    public void Die()
    {
        #if UNITY_EDITOR
        Debug.Log("Die");
        #endif
    }



}
