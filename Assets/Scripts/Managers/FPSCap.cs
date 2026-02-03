using UnityEngine;

public class FPSCap : MonoBehaviour
{
    [SerializeField] private int targetFPS = 60;
    
    void Start()
    {
        Application.targetFrameRate = targetFPS;
    }
    
    // Для изменения во время игры
    public void SetFPS(int fps)
    {
        Application.targetFrameRate = fps;
        Debug.Log($"FPS ограничен до {fps}");
    }
}