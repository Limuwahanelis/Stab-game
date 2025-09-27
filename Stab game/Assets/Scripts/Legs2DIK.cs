using System.Collections;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Legs2DIK : MonoBehaviour
{
    enum LastMovedLeg
    {
        LEFT=-1,RIGHT=1
    }
    public bool IsMoveing => _isStepping;
    [Header("Debug")]
    [SerializeField] bool _debug;
    [SerializeField] Transform _helper;
    [Space]
    [SerializeField] LayerMask _groundLayer;
    [SerializeField]Transform _helperTran;
    [SerializeField] float _speed;
    [SerializeField] Transform _middleOfTheBody;
    [SerializeField] float _maxDistanceFromBody;
    [SerializeField] float _maxDistanceBetweenLegs;
    [SerializeField] Transform _mainBody;
    [SerializeField] PlayerRaycasts2D _raycasts;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Transform _skeleton;
    [SerializeField] float _lowestDiff;
    [SerializeField] float _highestDiff;
    [Header("Raycasts")]
    [SerializeField] Transform _LLForwardRaycastTrans;
    [SerializeField] Transform _RLForwardRaycastTrans;
    [SerializeField] float _forwardRaycastLength;
    [SerializeField] float _downRaycastLength;
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
    private void Awake()
    {
        _skeletonStartingPos = _skeleton.localPosition;
        Vector3 tmp = _skeleton.localPosition;
        tmp.y -= _lowestDiff;
        //_skeleton.position = tmp;
        _skeletonLowestPos= tmp;
    }
    public RaycastHit2D RightLegRaycast()
    {
        Debug.DrawLine(_RLForwardRaycastTrans.position, _RLForwardRaycastTrans.position + _RLForwardRaycastTrans.up * _forwardRaycastLength,Color.magenta, 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(_RLForwardRaycastTrans.position, _RLForwardRaycastTrans.up, _forwardRaycastLength, _groundLayer);

        if (hit) return hit;

        _helperTran.position = _RLForwardRaycastTrans.position + _RLForwardRaycastTrans.up * _forwardRaycastLength;
        hit = Physics2D.Raycast(_helperTran.position, _helperTran.up, _downRaycastLength, _groundLayer);
        if (hit == false) hit.point = _RLTarget.position;
        return hit;
    }
    public RaycastHit2D LeftLegRaycast()
    {
        Debug.DrawLine(_LLForwardRaycastTrans.position, _LLForwardRaycastTrans.position + _LLForwardRaycastTrans.up * _forwardRaycastLength, Color.magenta, 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(_LLForwardRaycastTrans.position, _LLForwardRaycastTrans.up, _forwardRaycastLength, _groundLayer);

        if (hit) return hit;
        
        _helperTran.position = _LLForwardRaycastTrans.position + _LLForwardRaycastTrans.up * _forwardRaycastLength;
        hit = Physics2D.Raycast(_helperTran.position, _helperTran.up, _downRaycastLength, _groundLayer);
        if (hit == false) hit.point = _LLTarget.position;
        return hit;
    }
    public void Step()
    {
        if (_isStepping) return;
        _isStepping = true;
        if(_lastMovedLeg==LastMovedLeg.LEFT)
        {
            _lastMovedLeg=LastMovedLeg.RIGHT;
            StartCoroutine(MoveRightLeg());
        }
        else
        {
            _lastMovedLeg = LastMovedLeg.LEFT;
            StartCoroutine(MoveLeftLeg());
        }
    }
    public bool TryMoveLegs()
    {
        if(_isMovingLeftLeg || _isMovingRightLeg) return false;

        if(Vector2.Distance(_RLTarget.position,_middleOfTheBody.position)> _maxDistanceFromBody)
        {
            if (Vector2.Distance(_RLTarget.position, _LLTarget.position) > _maxDistanceBetweenLegs)
            {
                StartCoroutine(MoveRightLeg());
                return true;
            }
        }
        else if (Vector2.Distance(_LLTarget.position, _middleOfTheBody.position) > _maxDistanceFromBody)
        {
            if (Vector2.Distance(_RLTarget.position, _LLTarget.position) > _maxDistanceBetweenLegs)
            {
                StartCoroutine(MoveLeftLeg());
                return true;
            }
        }
        return false;
    }
    private void MoveBody(ref float distanceBetweenLegs, ref Vector3 pos)
    {
        distanceBetweenLegs = Vector2.Distance((Vector2)_RLTarget.position, (Vector2)_LLTarget.position);
        pos = Vector3.Lerp(_skeletonLowestPos, _skeletonStartingPos, 1 - distanceBetweenLegs / _maxDistanceBetweenLegs);
        pos.x = (_LLTarget.position.x + _RLTarget.position.x) / 2;
        _rb.MovePosition(new Vector3(pos.x, _raycasts.GroundHit.point.y));
        _skeleton.localPosition = new Vector3(0, pos.y, 0);
    }
    private IEnumerator MoveRightLeg()
    {
        _isMovingRightLeg = true;
        Vector2 startpos = _RLTarget.position;
        Vector2 targetPos = RightLegRaycast().point;
        float distanceBetweenLegs=0;
        float time = Vector2.Distance(targetPos, _RLTarget.position) / _speed;
        float t = 0;
        Vector3 pos = Vector3.zero;
        while (t<time)
        {
            _RLTarget.position = Vector3.Lerp(startpos, targetPos, t/ time);
            MoveBody(ref distanceBetweenLegs,ref pos);
             t += Time.deltaTime;
            yield return null;
        }
        _RLTarget.position = targetPos;
        MoveBody(ref distanceBetweenLegs, ref pos);
        _isMovingRightLeg = false;
        _isStepping = false;
    }

    private IEnumerator MoveLeftLeg()
    {
        _isMovingLeftLeg = true;
        Vector2 startpos = _LLTarget.position;
        Vector2 targetPos = LeftLegRaycast().point;
        float time = Vector2.Distance(targetPos, _LLTarget.position) / _speed;
        float t = 0;
        float distanceBetweenLegs=0;
        Vector3 pos=Vector3.zero;
        while (t < time)
        {
            _LLTarget.position = Vector3.Lerp(startpos, targetPos, t / time);
            MoveBody(ref  distanceBetweenLegs, ref  pos);
            t += Time.deltaTime;
            yield return null;
        }
        _LLTarget.position = targetPos;
        MoveBody(ref distanceBetweenLegs, ref pos);
        _isMovingLeftLeg = false;
        _isStepping = false;
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
        if (_LLForwardRaycastTrans) Gizmos.DrawLine(_LLForwardRaycastTrans.position, _LLForwardRaycastTrans.position+ _LLForwardRaycastTrans.up * _forwardRaycastLength);
        Gizmos.color = Color.blue;
        Vector3 pos = _middleOfTheBody.position;
        Gizmos.DrawLine(_middleOfTheBody.position + new Vector3(pos.x - _maxDistanceBetweenLegs / 2, -0.25f, 0), _middleOfTheBody.position + new Vector3(pos.x + _maxDistanceBetweenLegs / 2,- 0.25f, 0));
    }
}

