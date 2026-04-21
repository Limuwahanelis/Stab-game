using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement2DIK : MonoBehaviour
{
    enum LastStep
    {
        RIGHT_FORWARD, LEFT_FORWARD, RIGHT_BACK, LEFT_BACK
    }
    public int FlipSide => _flipSide;
    public bool IsPlayerFalling { get => _rb.linearVelocity.y < 0; }
    public Rigidbody2D PlayerRB => _rb;
    [Header("Common")]
    [SerializeField] Camera _cam;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] PlayerController _player;
    [SerializeField] Transform _toRotate;
    [SerializeField] Transform _raycastsTran;
    [SerializeField] float _normalGravityForce;
    [SerializeField] float _distanceTotravelForStep = 0.2f;
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
    private bool _isStandingLegToLeg = true;
    private bool _wasLastStepForward = false;
    private GlobalEnums.HorizontalDirections _upperBodyDirection = GlobalEnums.HorizontalDirections.RIGHT;
    private GlobalEnums.HorizontalDirections _lastStepDirection;
    public void MoveForward()
    {

    }
    private void Update()
    {
        _oldPlayerDirection = _newPlayerDirection;

        _newPlayerDirection = (_cam.ScreenToWorldPoint(HelperClass.MousePos).x < _player.MainBody.transform.position.x) ? GlobalEnums.HorizontalDirections.LEFT : GlobalEnums.HorizontalDirections.RIGHT;

        if (_newPlayerDirection == GlobalEnums.HorizontalDirections.RIGHT)
        {
            _flipSide = 1;
        }
        if (_newPlayerDirection == GlobalEnums.HorizontalDirections.LEFT)
        {
            _flipSide = -1;

        }
        if (!_ik.IsMoveing && _isStandingLegToLeg)
        {
            Vector3 scale = Vector3.one;
            scale.x = _flipSide;
            _RLIKTarget.localScale = scale;
            _LLIKTarget.localScale = scale;
            _toRotate.localScale = Vector3.one;
            _player.MainBody.transform.localScale = scale;
            _ik.SetDirection(_newPlayerDirection);
        }

        // TODO: Gdy obrucony na kucaku, obruc calego po backstepie

        if (_newPlayerDirection != _oldPlayerDirection)
        {

            if (_newPlayerDirection == GlobalEnums.HorizontalDirections.RIGHT)
            {
                _flipSide = 1;
            }
            if (_newPlayerDirection == GlobalEnums.HorizontalDirections.LEFT)
            {
                _flipSide = -1;
            }

            Vector3 scale = _toRotate.localScale;
            if (_ik.IsMoveing)
            {
                if (_player.MainBody.transform.localScale.x > 0)
                {
                    scale.x = _flipSide;
                    _toRotate.localScale = scale;
                }
                else
                {
                    scale.x = -_flipSide;
                    _toRotate.localScale = scale;
                }
            }
            else
            {
                if (_isStandingLegToLeg)
                {
                    scale.x = _flipSide;
                    _toRotate.localScale = Vector3.one;
                    _player.MainBody.transform.localScale = scale;
                    Logger.Log("Change targets scale");
                    _ik.SetDirection(_newPlayerDirection);
                    _RLIKTarget.localScale = scale;
                    _LLIKTarget.localScale = scale;
                }
                else
                {
                    if (_player.MainBody.transform.localScale.x > 0)
                    {
                        scale.x = _flipSide;
                        _toRotate.localScale = scale;
                    }
                    else
                    {
                        scale.x = -_flipSide;
                        _toRotate.localScale = scale;
                    }
                }
            }

        }
    }
    public void Move(Vector2 direction)
    {
        if (_ik.IsMoveing) return;
        if (direction.x > 0)
        {
            if (_isStandingLegToLeg)
            {
                if (_toRotate.lossyScale.x >= 0)
                {
                    _ik.Step();
                }
                else _ik.StepBack();
            }
            else
            {
                if (_toRotate.lossyScale.x >= 0)
                {
                    if (_player.MainBody.transform.localScale.x >= 0)
                    {
                        _ik.Step();
                    }
                    else _ik.StepBack();

                }
                else
                {
                    _ik.StepBack();
                }
            }

        }
        else
        {
            if (_isStandingLegToLeg)
            {
                if (_toRotate.lossyScale.x >= 0)
                {
                    _ik.StepBack();
                }
                else _ik.Step();
            }
            else
            {
                if (_toRotate.lossyScale.x >= 0)
                {
                    _ik.StepBack();
                }
                else
                {
                    if (_player.MainBody.transform.localScale.x >= 0)
                    {
                        _ik.StepBack();
                    }
                    else _ik.Step();

                }
            }
        }
        if (_isStandingLegToLeg) _isStandingLegToLeg = false;
        else _isStandingLegToLeg = true;

    }

    IEnumerator MoveCor(Vector2 newPos)
    {
        _isMoving = true;
        //_ik.Step();
        Vector2 startPos = _rb.position;
        float time = _distanceTotravelForStep / _speed;
        float t = 0;
        while (t < time)
        {
            Vector2 pos = Vector2.Lerp(startPos, newPos, t / time);
            t += Time.fixedDeltaTime;
            _rb.MovePosition(pos);
            yield return new WaitForFixedUpdate();
        }
        _rb.MovePosition(newPos);
        _isMoving = false;
    }
}
