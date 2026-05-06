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

        this.OnLongPress.AddListener(ShowSpellDescription);
        this.OnLongPressEnded.AddListener(HideSpellDescription);
    }

    public virtual Spell GetDefaultSpell()
    {
        return null;
    }

    protected override void OnClickAction()
    {
        base.OnClickAction();
        animator.StartOnClickAnimation();

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

    private void ShowSpellDescription()
    {
        var spellDesc = FindFirstObjectByType<SpellDescriptionBehaviour>();
        string spellName = SpellDescriptionBehaviour.ColorString(spell.Name, GetColorFromSpellRareness());
        string desc = $"{spellName}\n{spell.Description}";
        spellDesc.ShowDescription(desc);
    }

    private void HideSpellDescription()
    {
        var spellDesc = FindFirstObjectByType<SpellDescriptionBehaviour>();
        spellDesc.HideDescription();
    }

    public Vector3 GetButtonWorldPosition()
    {
        Vector3 screenPos = buttonRect.position;
        screenPos.z = 0f;
        return screenPos;
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