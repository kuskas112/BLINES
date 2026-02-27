using UnityEngine;
using UnityEngine.UI;

public abstract class ButtonBehaviour : MonoBehaviour
{
    public Button button;
    public bool Clickable = true;
    public bool isClicked = false;

    protected virtual void Awake()
    {
        if (button == null) button = GetComponent<Button>();
        button.onClick.AddListener(OnClickListener);
    }

    public virtual void OnClickListener()
    {
        if (!Clickable) return;
        if (isClicked) return;
        isClicked = true;
        PlayDefaultAnimation();
        PlaySpecificAnimation();
    }

    protected virtual void PlayDefaultAnimation() { }
    protected abstract void PlaySpecificAnimation();
}
