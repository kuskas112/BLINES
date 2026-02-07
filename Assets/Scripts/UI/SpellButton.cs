using UnityEngine;
using UnityEngine.UI;

public class SpellButton : MonoBehaviour
{
    public Button button;
    public GameObject spellObject;

    // Spell получается по индексу, который соответствует этому спеллу в списке спеллов игрока
    // Не знаю имеет ли смысл хранить ссылку на спелл, ведь список спеллов игрока меняется,
    // а кнопка всегда в ответе за конкретный индекс

    // Spell получается через метод GetSpell()
    [SerializeField] private int spellIndex = 0;
    private ShapeFacade spellFacade;
    private Camera mainCamera;
    private RectTransform buttonRect;
    private Image image;
    private Color _outlineColor;
    [SerializeField]
    public Color OutlineColor
    {
        get { return _outlineColor; }
        set 
        {
            _outlineColor = value; 
            UpdateOutlineColor();
        }
    }


    private void Awake(){
        if(button == null) button = GetComponent<Button>();
        buttonRect = button.GetComponent<RectTransform>();
        if(mainCamera == null) mainCamera = CameraManager.Camera;
        button.onClick.AddListener(OnClickListener);
        if(spellObject != null) SetShapeFacade();
        image = button.GetComponent<Image>();
        image.material = new Material(image.material);
        OutlineColor = Color.grey; // Изначальный цвет контура
    }

    private void SetShapeFacade()
    {
        spellFacade = new ShapeFacade(spellObject);
    }

    private void Start()
    {
        GetColorFromSpellRareness();
    }

    public void MoveObjectInsideButton()
    {
        Vector3 newPos = GetButtonWorldPosition();
        if(spellFacade != null)spellFacade.mover.Move(newPos, 1f);
    }

    public void OnClickListener(){
        // А как ты собрался кастовать если нету объекта спелла
        if(spellFacade != null) GetSpell().Cast(BattleManager.Instance.battleContext);
    }

    private Spell GetSpell()
    {
        return BattleManager.Instance.Player.spells[spellIndex];
    }

    private void UpdateOutlineColor()
    {
        image.material.SetColor("_NeonColor", OutlineColor);
    }

    Vector3 GetButtonWorldPosition()
    {
        Vector3 screenPos = buttonRect.position;
        screenPos.z = 0f;
        Vector3 worldPos = screenPos;
        return worldPos;
    }

    private void GetColorFromSpellRareness()
    {
        switch (GetSpell().Rareness)
        {
            case SpellRareness.Default:
                OutlineColor = Color.grey;
                break;
            case SpellRareness.Rare:
                OutlineColor = Color.blue;
                break;
            case SpellRareness.Epic:
                OutlineColor = Color.magenta;
                break;
            case SpellRareness.Legendary:
                OutlineColor = Color.yellow;
                break;
        }
    }
}