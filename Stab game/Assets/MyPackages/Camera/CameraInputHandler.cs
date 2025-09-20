using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraInputHandler : MonoBehaviour
{
    [SerializeField] PlayerCameraMouse _camera;
    bool _isRotating = false;
    float _rotValue = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseSettings.IsGamePaused) return;
        if (_isRotating)
        {
            _camera.RotateKeyboardX(_rotValue);
        }
    }
    void OnScroll(InputValue value)
    {
        if (PauseSettings.IsGamePaused) return;
        _camera.Scroll(value.Get<float>());
    }
    void OnMouseDelta(InputValue value)
    {
        if (PauseSettings.IsGamePaused) return;
        _camera.RotateMouse(value.Get<Vector2>().x, value.Get<Vector2>().y);
    }
    void OnMousePos(InputValue value)
    {
        HelperClass.SetMousePos(value.Get<Vector2>());
    }
    void OnRotateX(InputValue value)
    {
        _rotValue = value.Get<float>();
        
        if (math.abs(_rotValue) > 0) _isRotating = true;
        else _isRotating = false;

    }
    private void OnValidate()
    {
       // if (_camera == null) _camera = FindObjectOfType<PlayerCamera>();
    }
}
