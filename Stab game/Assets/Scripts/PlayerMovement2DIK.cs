using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement2DIK : MonoBehaviour
{
    public int FlipSide => _flipSide;
    public bool IsPlayerFalling { get => _rb.linearVelocity.y < 0; }
    public Rigidbody2D PlayerRB => _rb;
    [Header("Common")]
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] PlayerRaycasts2D _raycasts;
    [SerializeField] PlayerController _player;
    [SerializeField] float _normalGravityForce;
    [SerializeField] float _distanceTotravelForStep=0.2f;
    [SerializeField] float _speed;
    [Header("Jump")]
    [SerializeField] Ringhandle _jumpHandle;
    [SerializeField] SlopeDetection _slopeDetection;
    [SerializeField] float _jumpStrength;
    [Header("IK")]
    [SerializeField] Transform _RLIKTarget;
    [SerializeField] Transform _LLIKTarget;
    [SerializeField] Legs2DIK _ik;

    private int _flipSide = 1;
    private GlobalEnums.HorizontalDirections _newPlayerDirection;
    private GlobalEnums.HorizontalDirections _oldPlayerDirection;
    private float _previousDirection;
    private bool _isMoving = false;
    public void MoveForward()
    {

    }
    private void Update()
    {
        //Vector2 newPos = _rb.position;
        //newPos.y = _raycasts.GroundHit.point.y;
        //_rb.MovePosition(newPos);
    }
    public void Move(Vector2 direction)
    {
        if (_ik.IsMoveing) return;
        if (direction.x != 0)
        {
            _newPlayerDirection = (GlobalEnums.HorizontalDirections)direction.x;
            if (_newPlayerDirection != _oldPlayerDirection)
            {
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
                Vector2 tmp = _LLIKTarget.position;
                _LLIKTarget.position = _RLIKTarget.position;
                _RLIKTarget.position = tmp;
                Logger.Log("Rot");
            }
            else
            {
                Logger.Log("Step");
                _previousDirection = direction.x;
                _ik.Step();
            }
            //Vector2 newPos = _rb.position + new Vector2(_player.MainBody.transform.right.x * _flipSide * _distanceTotravelForStep, 0);

           // StartCoroutine(MoveCor(newPos));
            _oldPlayerDirection = _newPlayerDirection;
         
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
    IEnumerator MoveCor(Vector2 newPos)
    {
        _isMoving = true;
        //_ik.Step();
        Vector2 startPos = _rb.position;
        float time = _distanceTotravelForStep/_speed;
        float t = 0;
        while(t< time)
        {
            Vector2 pos = Vector2.Lerp(startPos, newPos, t/time);
            t += Time.fixedDeltaTime;
            _rb.MovePosition(pos);
            yield return new WaitForFixedUpdate();
        }
        _rb.MovePosition(newPos);
        _isMoving = false;
    }
}
