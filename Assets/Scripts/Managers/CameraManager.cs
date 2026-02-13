using UnityEngine;

public static class CameraManager
{
    private static Camera _camera;

    public static Camera Camera 
    {
        get
        {
            if (_camera == null)
            {
                _camera = Object.FindFirstObjectByType<Camera>();
            }
            return _camera;
        }
    }

    private static Transform _cameraTransform;
    public static Transform CameraTransform 
    {
        get
        {
            if (_cameraTransform == null)
            {
                _cameraTransform = Camera.transform;
            }
            return _cameraTransform;
        }
    }

    private static float _screenWidthWorld = 0;
    public static float ScreenWidthWorld
    {
        get 
        {
            if (_screenWidthWorld == 0)
            {
                _screenWidthWorld = GetScreenWidthWorld();
            }
            return _screenWidthWorld;
        }
    }

    public static float XWorldToPixelsRatio = Screen.width / ScreenWidthWorld;

    public static float WorldXToPixels(float worldX)
    {
        return worldX * XWorldToPixelsRatio;
    }

    private static float GetScreenWidthWorld() 
    {
        return Camera.orthographicSize * Camera.aspect * 2f;
    }

}

