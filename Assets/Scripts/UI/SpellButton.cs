using UnityEngine;
using UnityEngine.UI;

public class SpellButton : MonoBehaviour
{
    public Button button;
    public GameObject spellObject;
    private ShapeFacade spellFacade;
    private Camera mainCamera;
    private RectTransform buttonRect;

    private void Awake(){
        if(button == null) button = GetComponent<Button>();
        buttonRect = button.GetComponent<RectTransform>();
        if(mainCamera == null) mainCamera = CameraManager.Camera;
        button.onClick.AddListener(OnClickListener);
        spellFacade = new ShapeFacade(spellObject);
        
    }

    private void Start()
    {
        Vector3 newPos = GetButtonWorldPosition();
        spellFacade.mover.Move(newPos, 1f);
    }

    public void OnClickListener(){
        BattleManager.Instance.Player.spells[0].Cast(BattleManager.Instance.battleContext);
    }

    Vector3 GetButtonWorldPosition()
    {
        Vector3 screenPos = buttonRect.position;
        screenPos.z = 0f;
        Vector3 worldPos = screenPos;
        return worldPos;
    }
}
