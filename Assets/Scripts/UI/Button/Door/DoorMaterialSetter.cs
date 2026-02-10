using UnityEngine;

public class DoorMaterialSetter : ButtonMaterialSetter
{
    private float _outlineWidth;
    public float OutlineWidth
    {
        get { return _outlineWidth; }
        set 
        {
            _outlineWidth = value; 
            SetOutlineWidth(_outlineWidth);
        }
    }

    private float _pulseSpeed;
    public float PulseSpeed
    {
        get { return _pulseSpeed; }
        set
        {
            _pulseSpeed = value;
            SetPulseSpeed(_pulseSpeed);
        }
    }

    private void Start()
    {
        _outlineWidth = EdgeMaterial.GetFloat("_OutlineWidth");
        _pulseSpeed = EdgeMaterial.GetFloat("_PulseSpeed");
    }

    private void SetOutlineWidth(float width)
    {
        EdgeMaterial.SetFloat("_OutlineWidth", width);
    }

    private void SetPulseSpeed(float speed)
    {
        EdgeMaterial.SetFloat("_PulseSpeed", speed);
    }
}
