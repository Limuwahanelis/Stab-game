using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement2DIK : MonoBehaviour
{
    enum LastStep
    {
        RIGHT_FORWARD,LEFT_FORWARD,RIGHT_BACK,LEFT_BACK
    }
    public int FlipSide => _flipSide;
    public bool IsPlayerFalling { get => _rb.linearVelocity.y < 0; }
    public Rigidbody2D PlayerRB => _rb;
    [Header("Common")]
    [SerializeField] Camera _cam;
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
    private GlobalEnums.HorizontalDirections _lastStepDirection;
    public void MoveForward()
    {

    }
    private void Update()
    {
        _oldPlayerDirection = _newPlayerDirection;
        _newPlayerDirection = (_cam.ScreenToWorldPoint(HelperClass.MousePos).x < _player.MainBody.transform.position.x) ? GlobalEnums.HorizontalDirections.LEFT : GlobalEnums.HorizontalDirections.RIGHT;
        //Logger.Log((_cam.ScreenToWorldPoint(HelperClass.MousePos)));
        //Logger.Log(_newPlayerDirection);
        if (_newPlayerDirection != _oldPlayerDirection)
        {

            if (_newPlayerDirection == GlobalEnums.HorizontalDirections.RIGHT)
            {
                _flipSide = 1;
                _player.MainBody.transform.localScale = new Vector3(_flipSide, _player.MainBody.transform.localScale.y, _player.MainBody.transform.localScale.z);
            }
            if (_newPlayerDirection == GlobalEnums.HorizontalDirections.LEFT)
            {
                _flipSide = -1;
                _player.MainBody.transform.localScale = new Vector3(_flipSide, _player.MainBody.transform.localScale.y, _player.MainBody.transform.localScale.z);

            }
            Vector3 scale = _RLIKTarget.localScale;
            scale.x = _flipSide;
            _RLIKTarget.localScale = scale;
            _LLIKTarget.localScale = scale;
            //Vector2 tmp = _LLIKTarget.position;
            //_LLIKTarget.position = _RLIKTarget.position;
            //_RLIKTarget.position = tmp;
            Logger.Log(_flipSide);
        }


        //if (_cam.ScreenToWorldPoint(HelperClass.MousePos).x < _player.MainBody.transform.position.x)
        //{
        //    _flipSide = -1;
        //    _player.MainBody.transform.localScale = new Vector3(_flipSide, _player.MainBody.transform.localScale.y, _player.MainBody.transform.localScale.z);
        //}
        //else
        //{
        //    _flipSide = 1;
        //    _player.MainBody.transform.localScale = new Vector3(_flipSide, _player.MainBody.transform.localScale.y, _player.MainBody.transform.localScale.z);
        //}
        //Vector3 scale = _RLIKTarget.localScale;
        //scale.x = _flipSide;
        //_RLIKTarget.localScale = scale;
        //_LLIKTarget.localScale = scale;
        //Vector2 tmp = _LLIKTarget.position;
        //_LLIKTarget.position = _RLIKTarget.position;
        //_RLIKTarget.position = tmp;
        //Logger.Log("Rot");
    }
    public void Move(Vector2 direction)
    {
        if (_ik.IsMoveing) return;
        if(direction.x>0)
        {
            if ((_cam.ScreenToWorldPoint(HelperClass.MousePos).x < _player.MainBody.transform.position.x))
            {
                _lastStep = LastStep.RIGHT_BACK;
                _ik.StepBack();
                
            }
            else
            {
                _lastStep = LastStep.RIGHT_FORWARD;
                _ik.Step();
            }
        }
        else
        {
            if ((_cam.ScreenToWorldPoint(HelperClass.MousePos).x > _player.MainBody.transform.position.x))
            {
                _ik.StepBack();
            }
            else
            {
                _ik.Step();
            }
        }
        //if (direction.x != 0)
        //{
        //    _newPlayerDirection = (GlobalEnums.HorizontalDirections)direction.x;
        //    if (_newPlayerDirection != _oldPlayerDirection)
        //    {
        //        if (direction.x > 0)
        //        {
        //            _flipSide = 1;
        //            _player.MainBody.transform.localScale = new Vector3(_flipSide, _player.MainBody.transform.localScale.y, _player.MainBody.transform.localScale.z);
        //        }
        //        if (direction.x < 0)
        //        {
        //            _flipSide = -1;
        //            _player.MainBody.transform.localScale = new Vector3(_flipSide, _player.MainBody.transform.localScale.y, _player.MainBody.transform.localScale.z);

        //        }
        //        Vector3 scale = _RLIKTarget.localScale;
        //        scale.x = _flipSide;
        //        _RLIKTarget.localScale = scale;
        //        _LLIKTarget.localScale = scale;
        //        Vector2 tmp = _LLIKTarget.position;
        //        _LLIKTarget.position = _RLIKTarget.position;
        //        _RLIKTarget.position = tmp;
        //        Logger.Log("Rot");
        //    }
        //    else
        //    {
        //        Logger.Log("Step");
        //        _previousDirection = direction.x;
        //        _ik.Step();
        //    }
        //    //Vector2 newPos = _rb.position + new Vector2(_player.MainBody.transform.right.x * _flipSide * _distanceTotravelForStep, 0);

        //   // StartCoroutine(MoveCor(newPos));
        //    _oldPlayerDirection = _newPlayerDirection;

        //}
        //else
        //{
        //    //if (_previousDirection != 0)
        //    //{
        //    //    _rb.linearVelocity = new Vector2(0, 0);
        //    //    _rb.MovePosition(_rb.position + new Vector2(_mainBody.right.x * _flipSide * _speed * Time.deltaTime, 0));
        //    //}

        //}

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
