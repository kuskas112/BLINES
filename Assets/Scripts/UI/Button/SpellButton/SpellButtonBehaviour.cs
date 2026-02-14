using UnityEngine;
using UnityEngine.UI;

public class SpellButtonBehaviour : MonoBehaviour
{
    public Button button;
    public GameObject spellObject;
    public Spell spell;

    public bool Clickable = true;
    private ShapeFacade shapeFacade;
    private Camera mainCamera;
    private RectTransform buttonRect;


    private void Awake()
    {
        if (button == null) button = GetComponent<Button>();
        buttonRect = button.GetComponent<RectTransform>();
        if (mainCamera == null) mainCamera = CameraManager.Camera;
        button.onClick.AddListener(OnClickListener);
        if (spellObject != null) SetShapeFacade();
    }

    private void SetShapeFacade()
    {
        shapeFacade = spellObject.GetComponent<ShapeFacade>();
    }

    protected void SetSpell(Spell spell)
    {
        this.spell = spell;
        Color newColor = GetColorFromSpellRareness();
        GetComponent<MaterialSetter>().EdgeNeonColor = newColor;
    }

    public void MoveObjectInsideButton()
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

    public void OnClickListener()
    {
        // А как ты собрался кастовать если нету объекта спелла
        if (shapeFacade != null && BattleManager.Instance.IsPlayerTurn() && Clickable)
        {
            BattleManager.Instance.CastPlayerSpell(spell);
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