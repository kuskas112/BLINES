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

}

