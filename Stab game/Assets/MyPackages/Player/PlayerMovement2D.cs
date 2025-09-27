using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    public int FlipSide => _flipSide;
    public bool IsPlayerFalling { get => _rb.linearVelocity.y < 0; }
    public Rigidbody2D PlayerRB => _rb;
    [Header("Common")]
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Transform _mainBody;
    [SerializeField] PlayerController _player;
    [SerializeField] float _normalGravityForce;
    [SerializeField] float _speed;
    [Header("Jump")]
    [SerializeField] Ringhandle _jumpHandle;
    [SerializeField] SlopeDetection _slopeDetection;
    [SerializeField] float _jumpStrength;
    [Header("IK")]
    [SerializeField] Transform _RLIKTarget;
    [SerializeField] Transform _LLIKTarget;

    private int _flipSide = 1;
    private GlobalEnums.HorizontalDirections _newPlayerDirection;
    private GlobalEnums.HorizontalDirections _oldPlayerDirection;
    private float _previousDirection;
    public void MoveInAir(Vector2 direction,bool canRotate)
    {
        if (direction.x != 0)
        {
            _oldPlayerDirection = _newPlayerDirection;
            _newPlayerDirection = (GlobalEnums.HorizontalDirections)direction.x;
            _rb.MovePosition(_rb.position + new Vector2(_mainBody.right.x * _flipSide * _speed * Time.deltaTime * 0.6f, _rb.position.y));
            _previousDirection = direction.x;
        }
        else
        {
            if (_previousDirection != 0)
            {
                _rb.linearVelocity = new Vector2(0, 0);
                _rb.MovePosition(_rb.position + new Vector2(_mainBody.right.x * _flipSide * _speed * Time.deltaTime, 0));
                //StopPlayerOnXAxis();
            }

        }
    }
    public void MoveForward()
    {

    }
    public void Move(Vector2 direction,Vector2 groundRayHitPoint)
    {
       // if (!_slopeDetection.CanWalkOnSlope()) return;
        if (direction.x != 0)
        {
            _oldPlayerDirection = _newPlayerDirection;
            _newPlayerDirection = (GlobalEnums.HorizontalDirections)direction.x;
            Vector2 newPos = _rb.position + new Vector2(_mainBody.right.x * _flipSide * _speed * Time.deltaTime,0);
            newPos.y = groundRayHitPoint.y;
            //_rb.MovePosition(_rb.position + new Vector2(_mainBody.right.x * _flipSide * _speed * Time.deltaTime, -(_rb.position.y- groundRayHitPoint.y)));
            _rb.MovePosition(newPos);
            if (direction.x > 0)
            {
                _flipSide = 1;
                _player.MainBody.transform.localScale = new Vector3(_flipSide, _player.MainBody.transform.localScale.y, _player.MainBody.transform.localScale.z);
            }
            if (direction.x < 0)
            {
                _flipSide = -1;
                _player.MainBody.transform.localScale = new Vector3(_flipSide, _player.MainBody.transform.localScale.y, _player.MainBody.transform.localScale.z);

            }
            Vector3 scale = _RLIKTarget.localScale;
            scale.x = _flipSide;
            _RLIKTarget.localScale = scale;
            _LLIKTarget.localScale = scale;
            _previousDirection = direction.x;
        }
        else
        {
            //if (_previousDirection != 0)
            //{
            //    _rb.linearVelocity = new Vector2(0, 0);
            //    _rb.MovePosition(_rb.position + new Vector2(_mainBody.right.x * _flipSide * _speed * Time.deltaTime, 0));
            //}

        }

    }
}
