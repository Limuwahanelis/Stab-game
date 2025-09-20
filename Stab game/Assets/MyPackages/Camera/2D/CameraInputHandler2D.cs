using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class CameraInputHandler2D : MonoBehaviour
{
    [SerializeField] InputActionAsset _inputActions;
    [SerializeField] PlayerCamera2D _cam;
    [SerializeField] float _edgeSize;
    [SerializeField] float _camMoveSpeed;
    [SerializeField] bool _moveCamera;
    [SerializeField,Header("Raycasts")] bool _mouseRaycasts;
    [SerializeField, ConditionalField("_mouseRaycasts")] RaycastFromMouse2D _raycast;
    [SerializeField, ConditionalField("_mouseRaycasts")] InputActionReference _LMBAction;
    private Vector3 _keyboardCamMove;
    private bool _moveCameraByMouse = false;
    private bool _moveCameraByKeyboard = false;
    private void Start()
    {
        if (_mouseRaycasts)
        {
            _LMBAction.action.performed += LMB;
            _LMBAction.action.canceled += LMB;
            _LMBAction.action.started += LMB;
        }
    }
    // Update is called once per frameMov
    void Update()
    {
        if (_camMoveSpeed <= 0) return;
        if (_moveCameraByMouse) return;
        Vector3 pos = _cam.PositionToFollow;
        if (HelperClass.MousePos.x > Screen.width - _edgeSize) pos.x += _camMoveSpeed * Time.deltaTime;
        if (HelperClass.MousePos.x < 0 + _edgeSize) pos.x -= _camMoveSpeed * Time.deltaTime;
        if (HelperClass.MousePos.y > Screen.height-_edgeSize) pos.y += _camMoveSpeed * Time.deltaTime;
        if (HelperClass.MousePos.y < 0+_edgeSize) pos.y -= _camMoveSpeed * Time.deltaTime;
        if(_moveCameraByKeyboard)
        {
          pos += _keyboardCamMove * _camMoveSpeed * Time.deltaTime;
        }
        _cam.SetPositionToFollow(pos);
    }
    void OnMoveCamera(InputValue value)
    {
        if (!_moveCamera) return;
        if(value.Get<Vector2>()!=Vector2.zero) _moveCameraByKeyboard=true;
        else _moveCameraByMouse = false;
        Vector2 pos = _cam.PositionToFollow;
        //Logger.Log(value.Get<Vector2>());
        _keyboardCamMove = value.Get<Vector2>();
    }
    void OnMoveCameraByMouse(InputValue value)
    {
        if (!_moveCamera) return;
        if (value.Get<float>()>=1) _moveCameraByMouse = true;
        else _moveCameraByMouse = false;
    }
    void OnMouseDelta(InputValue value)
    {
        if (!_moveCamera) return;
        Vector2 pos = _cam.PositionToFollow;
        Vector2 delta= value.Get<Vector2>();
        if (_moveCameraByMouse)
        {
            pos -= delta* _camMoveSpeed * Time.deltaTime;
            _cam.SetPositionToFollowRaw(pos);
        }
    }
    void OnMousePos(InputValue value)
    {
        HelperClass.SetMousePos(value.Get<Vector2>());
    }
    public void LMB(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                if (context.interaction is HoldInteraction)
                {
                    _raycast.StartDrag();
                    Logger.Log("Hold");
                }
                else
                {
                    
                }
                break;

            case InputActionPhase.Canceled:
                _raycast.EndDrag();
                Logger.Log("Cancel");
                break;
        }
    }
    private void OnDestroy()
    {
        if (_mouseRaycasts)
        {
            _LMBAction.action.performed -= LMB;
            _LMBAction.action.canceled -= LMB;
            _LMBAction.action.started -= LMB;
        }
    }
}
