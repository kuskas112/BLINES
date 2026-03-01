using UnityEngine;
using UnityEngine.UI;

public class SpellButtonBehaviour : ButtonBehaviour
{
    public GameObject spellObject;
    public Spell spell;
    public ButtonAnimator animator;
    public bool Castable = true;

    private ShapeFacade shapeFacade;
    private Camera mainCamera;
    private RectTransform buttonRect;
    

    protected override void Awake()
    {
        base.Awake();
        if (animator == null) animator = GetComponent<ButtonAnimator>();
        if (buttonRect == null) buttonRect = button.GetComponent<RectTransform>();
        if (mainCamera == null) mainCamera = CameraManager.Camera;
        if (spellObject != null) SetShapeFacade();
        if (spell == null) SetSpell(GetDefaultSpell());
    }

    public virtual Spell GetDefaultSpell()
    {
        return null;
    }

    protected override void PlayDefaultAnimation()
    {
        base.PlayDefaultAnimation();
        animator.StartOnClickAnimation();
    }

    protected override void PlaySpecificAnimation()
    {
        
    }

    public virtual Spell GetSpell() 
    {
        if (spell == null) return GetDefaultSpell();
        return spell;
    }

    private void SetShapeFacade()
    {
        shapeFacade = spellObject.GetComponent<ShapeFacade>();
    }

    public void SetSpell(Spell spell)
    {
        this.spell = spell;
        Color newColor = GetColorFromSpellRareness();
        GetComponent<MaterialSetter>().EdgeNeonColor = newColor;
    }

    public void MoveObjectInsideButton()
    {
        Vector3 newPos = GetButtonWorldPosition();
        if(shapeFacade != null)shapeFacade.mover.SetPosition(newPos);
    }

    public void MoveObjectInsideButtonSmooth()
    {
        Vector3 newPos = GetButtonWorldPosition();
        if (shapeFacade != null) shapeFacade.mover.Move(newPos, 1f);
    }

    public void AddSpellToPlayer()
    {
        BattleManager.Instance.AddSpellToPlayer(spell);
    }

    public void AddSpellToEnemy()
    {
        BattleManager.Instance.AddSpellToEnemy(spell);
    }

    public override void OnClickListener()
    {
        base.OnClickListener();
        // А как ты собрался кастовать если нету объекта спелла
        if (shapeFacade != null && Clickable && Castable)
        {
            if (BattleManager.Instance.IsPlayerTurn())
            {
                BattleManager.Instance.CastPlayerSpell(spell);
            }
            else
            {
                BattleManager.Instance.CastEnemySpell(spell);
            }
        }
    }

    Vector3 GetButtonWorldPosition()
    {
        Vector3 screenPos = buttonRect.position;
        screenPos.z = 0f;
        Vector3 worldPos = screenPos;
        return worldPos;
    }

    private Color GetColorFromSpellRareness()
    {
        return spell.Rareness switch
        {
            SpellRareness.Default => Color.grey,
            SpellRareness.Rare => Color.blue,
            SpellRareness.Epic => Color.magenta,
            SpellRareness.Legendary => Color.yellow,
            _ => Color.clear,
        };
    }
}