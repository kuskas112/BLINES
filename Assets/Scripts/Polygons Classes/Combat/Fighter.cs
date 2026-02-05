using UnityEngine;

public class Fighter : MonoBehaviour
{
    public Spell[] spells =
    {
        new BasicAttack()
    };
    public PolygonFacade polygonFacade;
    public float Health = 100f;
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
