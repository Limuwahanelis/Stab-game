using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool IsPlayerFalling { get => _rb.linearVelocity.y < 0; }
    public Rigidbody PlayerRB => _rb;
    [Header("Common")]
    [SerializeField] Rigidbody _rb;
    [SerializeField] Transform _mainBody;
    [SerializeField] PlayerController _player;
    [SerializeField] Camera _mainCamera;
    [SerializeField] bool _takeCameraRotIntoAccount;
    [SerializeField,Header("Speeds")] float _walkSpeed=2f;
    [SerializeField] float _runSpeed=5f;
    [SerializeField] float _sprintSpeed=10f;
    [SerializeField,Header("Rotations")] float _angularRotationSpeed;
    private bool _isSprinting;
    private float _moveSpeed;
    private bool _canMove;
    private void Reset()
    {
        _mainCamera = Camera.main;
        _player = FindFirstObjectByType<PlayerController>();
    }
    public void Stop()
    {
        _rb.linearVelocity = Vector3.zero;
    }
    public void Move(Vector2 direction,GlobalEnums.MoveType moveType,bool snapRotation=true)
    {
        _canMove = true;
        if(!snapRotation)
        {
           
            _canMove = !IsRotating(direction);
        }
        else
        {
            Quaternion targetRot = Quaternion.identity;
            targetRot.eulerAngles = new Vector3(0, MathF.Atan2(direction.y, -direction.x) * (180 / Mathf.PI) - 90, 0);
            if (_takeCameraRotIntoAccount)
            {
                Quaternion camRot = Quaternion.identity;
                camRot.eulerAngles = new Vector3(0, _mainCamera.transform.rotation.eulerAngles.y, 0);
                targetRot *= camRot;
            }
            _rb.rotation = targetRot;
        }

        switch (moveType)
        {
            case GlobalEnums.MoveType.WALK: _moveSpeed=_walkSpeed; break;
            case GlobalEnums.MoveType.RUN: _moveSpeed = _runSpeed; break;
            case GlobalEnums.MoveType.SPRINT: _moveSpeed = _sprintSpeed; break;
        }

        if(_canMove)
        {

            //_rb.linearVelocity = new Vector3(direction.x * _moveSpeed, _rb.linearVelocity.y, direction.y * _moveSpeed);
            _rb.linearVelocity = _mainBody.forward*_moveSpeed;
        }
        else _rb.linearVelocity = Vector3.zero;
    }

    private bool IsRotating(Vector2 direction)
    {
        Quaternion targetRot = Quaternion.identity;
        targetRot.eulerAngles = new Vector3(0, MathF.Atan2(direction.y, -direction.x) * (180 / Mathf.PI) - 90, 0);
        if (_takeCameraRotIntoAccount)
        {
            Quaternion camRot = Quaternion.identity;
            camRot.eulerAngles = new Vector3(0, _mainCamera.transform.rotation.eulerAngles.y, 0);
            targetRot *= camRot;
        }
        _rb.rotation = Quaternion.RotateTowards(_rb.rotation, targetRot, Time.deltaTime * _angularRotationSpeed);
        if (Quaternion.Dot(_rb.rotation, targetRot) <= 0.99) return true;
        else return false;
    }
}
