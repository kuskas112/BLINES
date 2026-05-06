using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class CastAnimator
{
    public Spell spell;
    public CastAnimator(Spell spell)
    {
        this.spell = spell;
    }
    // Нужно ли по окончании анимации сменить ход?
    public bool NeedToSwitchTurn = true;
    public UnityEvent OnCastAnimationEnd   = new();
    public UnityEvent OnCastAnimationStart = new();
    public UnityEvent OnHit                = new();
    public Coroutine ActiveCoroutine = null;

    private bool isButtonUnlockListenerAdded = false;
    private bool isButtonLockListenerAdded   = false;

    abstract public IEnumerator CastAnimation(BattleContext context, float duration);

    public Coroutine StartCastAnimation(BattleContext context, float duration)
    {
        var button = FindSpellButton(context);

        if (!isButtonUnlockListenerAdded)
        {
            // Разблокировать кнопку после анимации
            OnCastAnimationEnd.AddListener(() =>
            {
                UnlockAllButtons(context);
            });
            isButtonUnlockListenerAdded = true;
        }

        if (!isButtonLockListenerAdded)
        {
            // Заблокировать кнопку до анимации
            OnCastAnimationStart.AddListener(() =>
            {
                LockAllButtons(context);
            });
            isButtonLockListenerAdded = true;
        }

        if (button != null)
        {
            ActiveCoroutine = button.StartCoroutine(CastAnimation(context, duration));
            return ActiveCoroutine;
        }
        else
        {
            Debug.LogError("Cannot start cast animation for spell " + this.spell.Name + " because no button was found.");
        }
        return null;
    }

    public void UnlockAllButtons(BattleContext context)
    {
        var allButtons = context.PlayerButtonsPlacer.spawnedInstances
                        .Concat(context.EnemyButtonsPlacer.spawnedInstances);
        foreach (var facade in allButtons)
        {
            facade.behaviour.UnlockButton();
        }
    }

    public void LockAllButtons(BattleContext context)
    {
        var allButtons = context.PlayerButtonsPlacer.spawnedInstances
                        .Concat(context.EnemyButtonsPlacer.spawnedInstances);
        foreach (var facade in allButtons)
        {
            facade.behaviour.LockButton();
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
