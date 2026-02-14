using UnityEngine;
public class Block : Spell
{
    public Block()
    {
        Name = "Block";
        Description = "Blocks next attacking spell";
        this.Type = SpellType.Active; 
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
        context.OnTurnStarted.AddListener(OnTurnStarted); // Listen for the end of the turn to reset defence
    }

    private void OnTurnStarted(int round) 
    {
        if (round - roundOfEffectStart != 1)
        {
            Debug.LogError("Effect of Block should last only for one turn, but it lasted for " + (round - roundOfEffectStart) + " turns.");
        }
        if (round > roundOfEffectStart)
        {
            caster.Defence = oldDefence; // Reset defence to original value
            Debug.Log("Defence reset to original value: " + oldDefence);
        }
        else
        {
            Debug.LogError("Turn started event triggered before the next turn. Current round: " + round + ", round of effect start: " + roundOfEffectStart);
        }
        context.OnTurnStarted.RemoveListener(OnTurnStarted); // Stop listening after resetting defence
    }
}
