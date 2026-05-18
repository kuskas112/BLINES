using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

public enum FighterType
{
    Player,
    Enemy
};
public class Fighter1fEvent : UnityEvent<float> {}
public class Fighter2fEvent : UnityEvent<float, float> {}
public class Fighter : MonoBehaviour
{
    public Fighter2fEvent onHealthChanged = new();
    public Fighter2fEvent onDefenceChanged = new();
    public List<Spell> spells = new();
    public PolygonFacade polygonFacade;
    public FighterType type = FighterType.Enemy;
    private float _health = 100f;
    public float Health
    {
        get { return _health; }
        set
        {
            float diff = value - _health;
            onHealthChanged.Invoke(value, diff);
            _health = value;
        }
    }
    
    private float _defence = 0f;
    public float Defence
    {
        get { return _defence; }
        set 
        {
            float diff = value - _defence;
            onDefenceChanged.Invoke(_defence, diff);
            _defence = Mathf.Clamp(value, 0f, 100f);
        }
    }

    public GameObject PolygonObject;

    void Awake()
    {
        if(PolygonObject == null)
        {
            // Если скрипт повесили прямо на полигон
            PolygonObject = gameObject;
        }
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
    }

    public void Die()
    {
        #if UNITY_EDITOR
        Debug.Log("Die");
        #endif
    }

    public List<Spell> GetAllPassiveSpells()
    {
        List<Spell> passiveSpells = new();
        foreach(var spell in spells)
        {
            if(spell.Type == SpellType.Passive)
            {
                passiveSpells.Add(spell);
            }
        }

        return passiveSpells;
    }

}
