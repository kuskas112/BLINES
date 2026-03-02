using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class CastAnimator
{
    public Spell spell;
    public CastAnimator(Spell spell)
    {
        this.spell = spell;
    }
    public UnityEvent OnCastAnimationEnd   = new();
    public UnityEvent OnCastAnimationStart = new();
    public UnityEvent OnHit                = new();


    abstract public IEnumerator CastAnimation(BattleContext context, float duration);

    public void StartCastAnimation(BattleContext context, float duration)
    {
        var button = FindSpellButton(context);
        if (button != null)
        {
            button.StartCoroutine(CastAnimation(context, duration));
        }
        else
        {
            Debug.LogError("Cannot start cast animation for spell " + this.spell.Name + " because no button was found.");
        }
    }

    protected SpellButtonFacade FindSpellButton(BattleContext context)
    {
        SpellButtonFacade obj = null;
        var buttons = context.IsPlayerTurn ? context.PlayerButtonsPlacer.spawnedInstances : context.EnemyButtonsPlacer.spawnedInstances;
        foreach (var facade in buttons)
        {
            if (facade.behaviour.GetSpell() == this.spell)
            {
                obj = facade.GetComponent<SpellButtonFacade>();
                break;
            }
        }

        if (obj == null)
        {
            Debug.LogError("No button found for spell " + this.spell.Name);
        }
        return obj;
    }
}
