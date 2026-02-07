using MyBox;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Legs2DIK : MonoBehaviour
{
    enum LastMovedLeg
    {
        LEFT=-1,NONE,RIGHT=1
    }
    public bool IsMoveing => _isStepping;
    [Header("Debug")]
    [SerializeField] bool _debug;
    [SerializeField,ConditionalField("_debug")]bool _leftLegDebug;
    [SerializeField, ConditionalField("_debug")] bool _downHill;
    [SerializeField] Transform _helper;
    [Space]
    [SerializeField] LayerMask _groundLayer;
    [SerializeField]Transform _helperTran;
    [SerializeField] float _speed;
    [SerializeField] Transform _middleOfTheBody;
    [SerializeField] float _maxDistanceFromBody;
    [SerializeField] float _maxDistanceBetweenLegs;
    [SerializeField] float _moveLegUpheight=0.3f;
    [SerializeField] Transform _mainBody;
    [SerializeField] PlayerRaycasts2D _raycasts;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Transform _skeleton;
    [SerializeField] float _lowestDiff;
    [SerializeField] float _highestDiff;
    [SerializeField] float _lowestSkeletonPosLocal = 0.25f;
    [Header("Raycasts")]
    [SerializeField] Transform _LLForwardRaycastTrans;
    [SerializeField] Transform _RLForwardRaycastTrans;
    [SerializeField] Transform _LLGroundRayTran;
    [SerializeField] Transform _RLGroundRayTran;
    [SerializeField] float _feetDownRayCast;
    [SerializeField] float _forwardRaycastLength;
    [SerializeField] float _downRaycastLength;
    [SerializeField] float _downhillForwardRaycastLength;
    [SerializeField] float _downhilldownRaycastLength;
    [Header("Effectors")]
    [SerializeField] Transform _LLEffector;
    [SerializeField] Transform _RLEffector;
    [Header("Targets")]
    [SerializeField] Transform _LLTarget;
    [SerializeField] Transform _RLTarget;


    private bool _isMovingLeftLeg;
    private bool _isMovingRightLeg;
    private Vector3 _skeletonStartingPos;
    private Vector3 _skeletonLowestPos;
    private bool _isStepping = false;
    private LastMovedLeg _lastMovedLeg = LastMovedLeg.LEFT;
    private bool _waslastMoveForward = true;
    private GlobalEnums.HorizontalDirections _forwardDir=GlobalEnums.HorizontalDirections.RIGHT;
    private GlobalEnums.HorizontalDirections _lastMoveDir=GlobalEnums.HorizontalDirections.RIGHT;
    private Vector3 _lowestCurrentSkeletonPos;
    private float _shortestCurrentDistanceBetweenLegs;
    private void Awake()
    {
        _skeletonStartingPos = _skeleton.localPosition;
        Vector3 tmp = _skeleton.localPosition;
        tmp.y -= _lowestDiff;
        //_skeleton.position = tmp;
        _lowestCurrentSkeletonPos = _skeletonLowestPos = tmp;
        _shortestCurrentDistanceBetweenLegs = _maxDistanceBetweenLegs;
    }
    public void SetDirection(GlobalEnums.HorizontalDirections direction)
    {
        _forwardDir = direction;
    }
    public RaycastHit2D RightLegRaycastBack()
    {
        float xOffset = _RLForwardRaycastTrans.localPosition.x;
        Vector3 newPos = _RLForwardRaycastTrans.position;
        if(_forwardDir==GlobalEnums.HorizontalDirections.LEFT) newPos.x += 2 * xOffset;
        if (_forwardDir == GlobalEnums.HorizontalDirections.RIGHT) newPos.x -= 2 * xOffset;
        Debug.DrawLine(newPos, newPos - _RLForwardRaycastTrans.up * _forwardRaycastLength, Color.magenta, 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(newPos, -_RLForwardRaycastTrans.up, _forwardRaycastLength, _groundLayer);
        if (hit) return hit;

        _helperTran.position = newPos - _RLForwardRaycastTrans.up * _forwardRaycastLength;
        hit = Physics2D.Raycast(_helperTran.position, _helperTran.up, _downRaycastLength, _groundLayer);
        if (hit == false) hit.point = _RLTarget.position;
        return hit;
    }
    public RaycastHit2D RightLegRaycast()
    {
        Debug.DrawLine(_RLForwardRaycastTrans.position, _RLForwardRaycastTrans.position + _RLForwardRaycastTrans.up * _forwardRaycastLength,Color.magenta, 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(_RLForwardRaycastTrans.position, _RLForwardRaycastTrans.up, _forwardRaycastLength, _groundLayer);
        
        if (hit) return hit;

        _helperTran.position = _RLForwardRaycastTrans.position + _RLForwardRaycastTrans.up * _forwardRaycastLength;
        hit = Physics2D.Raycast(_helperTran.position, _helperTran.up, _downRaycastLength, _groundLayer);
        if (hit == false)
        {
            hit = Physics2D.Raycast(_RLForwardRaycastTrans.position, _RLForwardRaycastTrans.up, _downhillForwardRaycastLength, _groundLayer);
            if (hit) return hit;
            _helperTran.position = _RLForwardRaycastTrans.position + _RLForwardRaycastTrans.up * _downhillForwardRaycastLength;
            hit = Physics2D.Raycast(_helperTran.position, _helperTran.up, _downhilldownRaycastLength, _groundLayer);

            if (hit == false) hit.point = _RLTarget.position;

        }
        return hit;
    }
    public RaycastHit2D LeftLegRaycastBack()
    {
        float xOffset = _LLForwardRaycastTrans.localPosition.x;
        Vector3 newPos = _LLForwardRaycastTrans.position;
        if (_forwardDir == GlobalEnums.HorizontalDirections.LEFT) newPos.x += 2 * xOffset;
        if (_forwardDir == GlobalEnums.HorizontalDirections.RIGHT) newPos.x -= 2 * xOffset;
        Debug.DrawLine(newPos, newPos - _LLForwardRaycastTrans.up * _forwardRaycastLength, Color.magenta, 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(newPos, -_LLForwardRaycastTrans.up, _forwardRaycastLength, _groundLayer);

        if (hit) return hit;

        _helperTran.position = newPos - _LLForwardRaycastTrans.up * _forwardRaycastLength;
        hit = Physics2D.Raycast(_helperTran.position, _helperTran.up, _downRaycastLength, _groundLayer);
        if (hit == false) hit.point = _LLTarget.position;
        return hit;
    }
    public RaycastHit2D LeftLegRaycast()
    {
        Debug.DrawLine(_LLForwardRaycastTrans.position, _LLForwardRaycastTrans.position + _LLForwardRaycastTrans.up * _forwardRaycastLength, Color.magenta, 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(_LLForwardRaycastTrans.position, _LLForwardRaycastTrans.up, _forwardRaycastLength, _groundLayer);

        if (hit) return hit;
        
        _helperTran.position = _LLForwardRaycastTrans.position + _LLForwardRaycastTrans.up * _forwardRaycastLength;
        hit = Physics2D.Raycast(_helperTran.position, _helperTran.up, _downRaycastLength, _groundLayer);
        if (hit == false)
        {
            hit = Physics2D.Raycast(_LLForwardRaycastTrans.position, _LLForwardRaycastTrans.up, _downhillForwardRaycastLength, _groundLayer);

            if (hit) return hit;

            _helperTran.position = _LLForwardRaycastTrans.position + _LLForwardRaycastTrans.up * _downhillForwardRaycastLength;
            hit = Physics2D.Raycast(_helperTran.position, _helperTran.up, _downhilldownRaycastLength, _groundLayer);

            if (hit == false)   hit.point = _LLTarget.position;
        }
        return hit;
    }
    public void StepBack()
    {
        if (_isStepping) return;
        _isStepping = true;
        if(_waslastMoveForward)
        {
            if (_lastMovedLeg == LastMovedLeg.LEFT)
            {
                if(_forwardDir!=_lastMoveDir)
                {
                    _lastMovedLeg = LastMovedLeg.RIGHT;
                    StartCoroutine(MoveRightLeg(false));
                }
                else
                {
                    _lastMovedLeg = LastMovedLeg.LEFT;
                    StartCoroutine(MoveLeftLeg(false));
                }

            }
            else
            {
                if (_forwardDir != _lastMoveDir)
                {
                    _lastMovedLeg = LastMovedLeg.LEFT;
                    StartCoroutine(MoveLeftLeg(false));
                }
                else
                {
                    _lastMovedLeg = LastMovedLeg.RIGHT;
                    StartCoroutine(MoveRightLeg(false));
                
                }
                 //   _lastMovedLeg = LastMovedLeg.RIGHT;
                //StartCoroutine(MoveRightLeg(false));
            }
        }
        else
        {
            if (_lastMovedLeg == LastMovedLeg.LEFT)
            {
                if (_lastMoveDir == _forwardDir)
                {
                    _lastMovedLeg = LastMovedLeg.LEFT;
                    StartCoroutine(MoveLeftLeg(false));
                    _waslastMoveForward = false;
                }
                else
                {
                    _lastMovedLeg = LastMovedLeg.RIGHT;
                    StartCoroutine(MoveRightLeg(false));
                    _waslastMoveForward = false;
                }

               // _lastMovedLeg = LastMovedLeg.RIGHT;
                //StartCoroutine(MoveRightLeg(false));
                

            }
            else
            {
                if (_forwardDir == _lastMoveDir)
                {
                    _lastMovedLeg = LastMovedLeg.RIGHT;
                    StartCoroutine(MoveRightLeg(false));
                    _waslastMoveForward = false;

                }
                else
                {
                    _lastMovedLeg = LastMovedLeg.LEFT;
                    StartCoroutine(MoveLeftLeg(false));
                    _waslastMoveForward = false;
                }
                //_lastMovedLeg = LastMovedLeg.LEFT;
                //StartCoroutine(MoveLeftLeg(false));
            }
           
        }
        _lastMoveDir =  (GlobalEnums.HorizontalDirections)(-1*(int)_forwardDir);
        _waslastMoveForward = false;
        // _waslastMoveForward = false;
    }
    public void Step()
    {
        if (_isStepping) return;
        _isStepping = true;
       
        if (_waslastMoveForward)
        {
            if (_lastMovedLeg == LastMovedLeg.LEFT)
            {
                // we moved forward left leg, rotated and want to move forward - bring legs together
                if(_lastMoveDir!=_forwardDir)
                {
                    _lastMovedLeg = LastMovedLeg.LEFT;
                    StartCoroutine(MoveLeftLeg(true));
                }
                else
                {
                    _lastMovedLeg = LastMovedLeg.RIGHT;
                    StartCoroutine(MoveRightLeg(true));
                }

            }
            else
            {
                // we moved forward left Right, rotated and want to move forward - bring legs together
                if (_lastMoveDir != _forwardDir)
                {
                    _lastMovedLeg = LastMovedLeg.RIGHT;
                    StartCoroutine(MoveRightLeg(true));
                }
                else
                {
                    _lastMovedLeg = LastMovedLeg.LEFT;
                    StartCoroutine(MoveLeftLeg(true));
                }
            }
        }
        else
        {
            if (_lastMovedLeg == LastMovedLeg.LEFT)
            {
                if(_lastMoveDir==_forwardDir)
                {
                    _lastMovedLeg = LastMovedLeg.RIGHT;
                    StartCoroutine(MoveRightLeg(true));
                }
                else
                {
                    _lastMovedLeg = LastMovedLeg.LEFT;
                    StartCoroutine(MoveLeftLeg(true));

                }
                //_lastMovedLeg = LastMovedLeg.LEFT;
                //StartCoroutine(MoveLeftLeg(true));
            }
            else
            {
                if (_lastMoveDir == _forwardDir)
                {
                    _lastMovedLeg = LastMovedLeg.LEFT;
                    StartCoroutine(MoveLeftLeg(true));
                }
                else
                {
                    _lastMovedLeg = LastMovedLeg.RIGHT;
                    StartCoroutine(MoveRightLeg(true));
                }
                //_lastMovedLeg = LastMovedLeg.RIGHT;
                //StartCoroutine(MoveRightLeg(true));
            }
        }
        _lastMoveDir = _forwardDir;
        _waslastMoveForward = true;
    }
    public bool TryMoveLegsBack()
    {
        if (_isMovingLeftLeg || _isMovingRightLeg) return false;

        if (Vector2.Distance(_RLTarget.position, _middleOfTheBody.position) > _maxDistanceFromBody)
        {
            if (Vector2.Distance(_RLTarget.position, _LLTarget.position) > _maxDistanceBetweenLegs)
            {
                StartCoroutine(MoveRightLeg(false));
                return true;
            }
        }
        else if (Vector2.Distance(_LLTarget.position, _middleOfTheBody.position) > _maxDistanceFromBody)
        {
            if (Vector2.Distance(_RLTarget.position, _LLTarget.position) > _maxDistanceBetweenLegs)
            {
                StartCoroutine(MoveLeftLeg(false));
                return true;
            }
        }
        return false;
    }
    public bool TryMoveLegs()
    {
        if(_isMovingLeftLeg || _isMovingRightLeg) return false;

        if(Vector2.Distance(_RLTarget.position,_middleOfTheBody.position)> _maxDistanceFromBody)
        {
            if (Vector2.Distance(_RLTarget.position, _LLTarget.position) > _maxDistanceBetweenLegs)
            {
                StartCoroutine(MoveRightLeg(true));
                return true;
            }
        }
        else if (Vector2.Distance(_LLTarget.position, _middleOfTheBody.position) > _maxDistanceFromBody)
        {
            if (Vector2.Distance(_RLTarget.position, _LLTarget.position) > _maxDistanceBetweenLegs)
            {
                StartCoroutine(MoveLeftLeg(true));
                return true;
            }
        }
        return false;
    }
    private void MoveBody(ref float distanceBetweenLegs, ref Vector3 pos,float legYDiff)
    {
        distanceBetweenLegs = Vector2.Distance((Vector2)_RLTarget.position, (Vector2)_LLTarget.position);
        pos = Vector3.Lerp(_lowestCurrentSkeletonPos, _skeletonStartingPos, 1 - distanceBetweenLegs / _shortestCurrentDistanceBetweenLegs);
        if(legYDiff<0)
        {
            Logger.Log("dif lower "+legYDiff);
            //pos.y += legYDiff;
        }
        pos.x = (_LLTarget.position.x + _RLTarget.position.x) / 2;
        _rb.MovePosition(new Vector3(pos.x, _raycasts.GroundHit.point.y));
        _skeleton.localPosition = new Vector3(0, pos.y, 0);
    }
    private float GetLegUpDistance(Vector3 startPos,Vector3 targetPos)
    {
        if (targetPos.y > startPos.y)
        {
            float diff = targetPos.y - startPos.y;
            return (_moveLegUpheight - diff);
        }
        else return _moveLegUpheight;
    }
    private IEnumerator MoveRightLeg(bool moveForward)
    {
        //Logger.Log("moving right leg");
        _isMovingRightLeg = true;
        Vector2 startpos = _RLTarget.position;
        Vector2 targetPos2 = moveForward?RightLegRaycast().point: RightLegRaycastBack().point;
        Vector2 targetPos1 = moveForward ? RightLegRaycast().point : RightLegRaycastBack().point;

        // if(_forwardDir==GlobalEnums.HorizontalDirections.RIGHT)
        //if (_RLTarget.localPosition.x < _LLTarget.localPosition.x) _shortestCurrentDistanceBetweenLegs = Vector3.Distance(_LLTarget.position, _RLTarget.position);
        //else
        //{
        //    if (Vector3.Distance(_LLTarget.position, targetPos2) < _maxDistanceBetweenLegs) _shortestCurrentDistanceBetweenLegs = Vector3.Distance(_LLTarget.position, targetPos2);
        //    else _shortestCurrentDistanceBetweenLegs = _maxDistanceBetweenLegs;
        //}
        if (Vector2.Distance(_LLTarget.position, targetPos2) < 0.200f)
        {
            targetPos2 = targetPos1 = _LLTarget.position;
            _shortestCurrentDistanceBetweenLegs = Vector3.Distance(_LLTarget.position, _RLTarget.position);

        }
        else
        {
            if (Vector3.Distance(_LLTarget.position, targetPos2) < _maxDistanceBetweenLegs) _shortestCurrentDistanceBetweenLegs = Vector3.Distance(_LLTarget.position, targetPos2);
            else _shortestCurrentDistanceBetweenLegs = _maxDistanceBetweenLegs;
        }
        targetPos1.x = (startpos.x + targetPos1.x) / 2;
        targetPos1.y += GetLegUpDistance(startpos,targetPos2);
        float distanceBetweenLegs=0;
        float time2 = Vector2.Distance(targetPos2, targetPos1) / _speed;
        float time1 = Vector2.Distance(targetPos1, _RLTarget.position) / _speed;
        float t = 0;
        float legYDiff=0;
        Vector3 pos = Vector3.zero;
        RaycastHit2D hit;
        while (t<time2+time1)
        {
            if (t < time1)
            {
                _RLTarget.position = Vector3.Lerp(startpos, targetPos1, t / time1);


            }
            else
            {
                //if (hit = Physics2D.Raycast(_RLGroundRayTran.position, Vector2.down, _feetDownRayCast, _groundLayer)) _RLTarget.position = hit.point;
                _RLTarget.position = Vector3.Lerp(targetPos1, targetPos2, (t - time1) / time2);
            }
            if(moveForward && _RLTarget.position.y < _LLTarget.position.y)
            {
                legYDiff = _RLTarget.position.y-_LLTarget.position.y ;
            }
            MoveBody(ref distanceBetweenLegs,ref pos,legYDiff);
             t += Time.deltaTime;
            yield return null;
        }
        if (moveForward && _RLTarget.position.y < _LLTarget.position.y)
        {
            legYDiff = _RLTarget.position.y - _LLTarget.position.y;
        }
        _RLTarget.position = targetPos2;
        MoveBody(ref distanceBetweenLegs, ref pos, legYDiff);
        _isMovingRightLeg = false;
        _isStepping = false;
        if(_skeleton.localPosition.y<_skeletonLowestPos.y)
        {
            _lowestCurrentSkeletonPos.y = _skeleton.localPosition.y;
        }
        else
        {
            _lowestCurrentSkeletonPos = _skeletonLowestPos;
        }

    }

    private IEnumerator MoveLeftLeg(bool moveForward)
    {
        Logger.Log("moving left leg");
        _isMovingLeftLeg = true;
        Vector2 startpos = _LLTarget.position;
        Vector2 targetPos2 = moveForward?LeftLegRaycast().point: LeftLegRaycastBack().point;
        Vector2 targetPos1 = moveForward ? LeftLegRaycast().point : LeftLegRaycastBack().point;

        // if left leg is behind right set _shortestCurrentDistanceBetweenLegs to distance between current left leg and right leg 
        //if (_LLTarget.localPosition.x < _RLTarget.localPosition.x) _shortestCurrentDistanceBetweenLegs = Vector3.Distance(_LLTarget.position, _RLTarget.position);
        //else
        //{
        //    if (Vector3.Distance(targetPos2, _RLTarget.position) < _maxDistanceBetweenLegs) _shortestCurrentDistanceBetweenLegs = Vector3.Distance(targetPos2, _RLTarget.position);
        //    else _shortestCurrentDistanceBetweenLegs = _maxDistanceBetweenLegs;
        //}
        if (Vector2.Distance(_RLTarget.position, targetPos2) < 0.200f)
        {
            targetPos2 = targetPos1 = _RLTarget.position;
            _shortestCurrentDistanceBetweenLegs = Vector3.Distance(_LLTarget.position, _RLTarget.position);
        }
        else
        {
            if (Vector3.Distance(targetPos2, _RLTarget.position) < _maxDistanceBetweenLegs) _shortestCurrentDistanceBetweenLegs = Vector3.Distance(targetPos2, _RLTarget.position);
            else _shortestCurrentDistanceBetweenLegs = _maxDistanceBetweenLegs;
        }
        targetPos1.x = (startpos.x + targetPos1.x) / 2;
        targetPos1.y += GetLegUpDistance(startpos, targetPos2);
        float distanceBetweenLegs = 0;
        float time2 = Vector2.Distance(targetPos2, targetPos1) / _speed;
        float time1 = Vector2.Distance(targetPos1, _LLTarget.position) / _speed;
        float t = 0;
        float legYDiff = 0;
        Vector3 pos = Vector3.zero;
        while (t < time2 + time1)
        {
            if (t < time1) _LLTarget.position = Vector3.Lerp(startpos, targetPos1, t / time1);
            else _LLTarget.position = Vector3.Lerp(targetPos1, targetPos2, (t - time1) / time2);
            if (moveForward && _LLTarget.position.y<_RLTarget.position.y)
            {
                legYDiff = _LLTarget.position.y-_RLTarget.position.y ;
            }
            MoveBody(ref distanceBetweenLegs, ref pos, legYDiff);
            t += Time.deltaTime;
            yield return null;
        }
        _LLTarget.position = targetPos2;
        if (moveForward && _LLTarget.position.y < _RLTarget.position.y)
        {
            legYDiff = _LLTarget.position.y - _RLTarget.position.y;
        }
        MoveBody(ref distanceBetweenLegs, ref pos, legYDiff);
        _isMovingLeftLeg = false;
        _isStepping = false;
        if (_skeleton.localPosition.y < _skeletonLowestPos.y)_lowestCurrentSkeletonPos.y = _skeleton.localPosition.y;
        else _lowestCurrentSkeletonPos = _skeletonLowestPos;

    }

    private void OnDrawGizmos()
    {
        if(!_debug) return;
        if(_middleOfTheBody)
        {
            Gizmos.DrawWireSphere(_middleOfTheBody.position, _maxDistanceFromBody);
        }
        Gizmos.color = Color.red;
        if (_RLForwardRaycastTrans)
        {
            Vector2 endpoint = _RLForwardRaycastTrans.position + _RLForwardRaycastTrans.up * _forwardRaycastLength;
            Gizmos.DrawLine(_RLForwardRaycastTrans.position, endpoint);
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(endpoint, (Vector3)endpoint +_helperTran.up * _downRaycastLength);
        }
        Gizmos.color = Color.red;
        if (_LLForwardRaycastTrans && _leftLegDebug) Gizmos.DrawLine(_LLForwardRaycastTrans.position, _LLForwardRaycastTrans.position+ _LLForwardRaycastTrans.up * _forwardRaycastLength);

        Gizmos.color = Color.green;
        if (_LLGroundRayTran) Gizmos.DrawLine(_LLGroundRayTran.position, _LLGroundRayTran.position + Vector3.down * _feetDownRayCast);
        if (_RLGroundRayTran) Gizmos.DrawLine(_RLGroundRayTran.position, _RLGroundRayTran.position + Vector3.down * _feetDownRayCast);

        Gizmos.color = Color.magenta;

        Gizmos.DrawWireSphere(_LLForwardRaycastTrans.position, MathF.Sqrt(_forwardRaycastLength * _forwardRaycastLength + _downRaycastLength * _downRaycastLength));

        Gizmos.color = Color.gray;

        Gizmos.DrawWireSphere(_LLForwardRaycastTrans.position, MathF.Sqrt(_downhillForwardRaycastLength * _downhillForwardRaycastLength + _downhilldownRaycastLength * _downhilldownRaycastLength));

        if (_downHill)
        {
            if (_RLForwardRaycastTrans)
            {
                Gizmos.color = Color.red;
                Vector2 endpoint = _RLForwardRaycastTrans.position + _RLForwardRaycastTrans.up * _downhillForwardRaycastLength;
                Gizmos.DrawLine(_RLForwardRaycastTrans.position, endpoint);
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(endpoint, (Vector3)endpoint + _helperTran.up * _downhilldownRaycastLength);
            }
        }
    }

    [CustomEditor(typeof(Legs2DIK))]
    class Legs2DIKEitro:Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if(GUILayout.Button("step"))
            {
                (target as Legs2DIK).Step();
            }
            if (GUILayout.Button("back step"))
            {
                (target as Legs2DIK).StepBack();
            }
        }
    }
}

