using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ButtonBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button button;
    public bool Clickable = true;
    public bool isClicked = false;

    public float holdTime = 0.5f;
    public UnityEvent OnLongPress;
    public UnityEvent OnLongPressEnded;

    private bool isPointerDown = false;
    private float pressStartTime;
    public bool longPressTriggered = false;

    protected virtual void Awake()
    {
        if (button == null) button = GetComponent<Button>();
        button.onClick.AddListener(OnClickListener);
    }

    public virtual void OnClickListener()
    {
        if (!Clickable) return;
        if (IsButtonLocked()) return;
        if (longPressTriggered) return;
        LockButton();
        OnClickAction();
    }

    public bool IsButtonLocked()
    {
        return isClicked;
    }

    public void LockButton()
    {
        isClicked = true;
    }

    public void UnlockButton()
    {
        isClicked = false;
    }

    protected virtual void OnClickAction() { }

    void Update()
    {
        if (isPointerDown && !longPressTriggered)
        {
            if (Time.time - pressStartTime >= holdTime)
            {
                longPressTriggered = true;
                OnLongPress?.Invoke();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        pressStartTime = Time.time;
        longPressTriggered = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        if (longPressTriggered)
        {
            OnLongPressEnded?.Invoke();
        }
    }
}
