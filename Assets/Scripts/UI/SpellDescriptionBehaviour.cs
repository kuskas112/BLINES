using UnityEngine;
using UnityEngine.UI;

public class SpellDescriptionBehaviour : MonoBehaviour
{
    [SerializeField]
    private Text text;
    private Renderer spriteRenderer;

    private void Awake()
    {
        if(text == null) text = GetComponentInChildren<Text>();
        text.text = string.Empty;
        spriteRenderer = GetComponent<Renderer>();
        spriteRenderer.enabled = false;
    }

    public void ShowDescription(string desc)
    {
        SetDescription(desc);
        spriteRenderer.enabled = true;
    }

    public void HideDescription()
    {
        SetDescription(string.Empty);
        spriteRenderer.enabled = false;
    }

    private void SetDescription(string desc) 
    { 
        text.text = desc;
    }

    public static string ColorString(string str, Color color)
    {
        string hexColor = ColorUtility.ToHtmlStringRGB(color);
        return $"<color=#{hexColor}>{str}</color>";
    }
}
