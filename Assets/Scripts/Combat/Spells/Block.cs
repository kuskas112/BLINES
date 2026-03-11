using UnityEngine;
public class Block : Spell
{
    public Block()
    {
        Name = "Block";
        Description = "Blocks next attacking spell";
        this.Type = SpellType.Active;
        CastAnimator = new OnCasterBounceCastAnimator(this);
    }
    private int roundOfEffectStart = 0;
    private float oldDefence;
    private Fighter caster;
    private BattleContext context;

    public override void Cast(BattleContext context)
    {
        base.Cast(context);
        this.context = context;
        caster = context.IsPlayerTurn ? context.Player : context.Enemy;
        roundOfEffectStart = context.Round;
        oldDefence = caster.Defence; // Store the original defence value
        caster.Defence = 100f; // Set defence to 100% for the next attack
        context.OnTurnSwitched.AddListener(OnTurnStarted); // Listen for the end of the turn to reset defence
        CastAnimator.StartCastAnimation(context, 2.5f);
    }

    private void OnTurnStarted(int round) 
    {
        if (round - roundOfEffectStart == 2)
        {
            caster.Defence = oldDefence; // Reset defence to original value
            Debug.Log("Defence reset to original value: " + oldDefence);
            context.OnTurnSwitched.RemoveListener(OnTurnStarted); // Stop listening after resetting defence
        }
    }
}
