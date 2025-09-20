using MyBox;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Vector3 PositionToFollow => _positionToFollow;

    [Header("Debug")]
    [SerializeField] bool _debug;

    [Header("Common")]
    [SerializeField, Tooltip("If set camera will try to always follow tis transform")] Transform _transformToFollow;
    [SerializeField] bool _takeStartPosAsOffset=true;
    [SerializeField,ConditionalField("_takeStartPosAsOffset",inverse:true)] Vector3 offset;

    [Header("Borders")]
    [SerializeField] bool _checkForBorders = true;
    [SerializeField,ConditionalField("_checkForBorders")] Transform leftScreenBorder;
    [SerializeField, ConditionalField("_checkForBorders")] Transform rightScreenBorder;
    [SerializeField, ConditionalField("_checkForBorders")] Transform upperScreenBorder;
    [SerializeField, ConditionalField("_checkForBorders")] Transform lowerScreenBorder;
    [SerializeField, ConditionalField("_checkForBorders")] Transform forwardScreenBorder;
    [SerializeField, ConditionalField("_checkForBorders")] Transform backScreenBorder;

    [Header("Smoothness")]
    [SerializeField] float _distanceToStartFollow;
    [SerializeField] float _smoothTime = 0.3f;

    private bool _isFollowing = false;
    private bool _followOnXAxis = true;
    private bool _followOnYAxis = true;
    private bool _followOnZAxis = true;
    private Vector3 _targetPos;
    private Vector3 _positionToFollow;
    //private float _horizontalMax;
    //private float _verticalMax;
    private Vector3 _velocity = Vector3.zero;
    private void Awake()
    {
        if(_takeStartPosAsOffset)
        {
            offset = transform.position-PositionToFollow;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //_horizontalMax = Camera.main.orthographicSize * Screen.width / Screen.height;
        //_verticalMax = Camera.main.orthographicSize;
        if (_transformToFollow) _positionToFollow = _transformToFollow.position;
        else _positionToFollow = transform.position;
        transform.position = _transformToFollow.position + offset;
    }
    private void Update()
    {
        if(Vector3.Distance(_transformToFollow.position, _positionToFollow)>_distanceToStartFollow) _isFollowing = true;
        if (_isFollowing == false) return;
        if (_transformToFollow) SetPositionToFollow(_transformToFollow.position);
    }
    private void FixedUpdate()
    {
        if (_transformToFollow == null || !_isFollowing) return;
        if (_checkForBorders)
        {
            _targetPos = _positionToFollow;
            if (_followOnXAxis)
            {
                _targetPos = new Vector3(_positionToFollow.x, _targetPos.y, _positionToFollow.z);
            }
            if (_followOnYAxis)
            {
                _targetPos = new Vector3(_targetPos.x, _positionToFollow.y, _positionToFollow.z);
            }
            if(_followOnZAxis)
            {
                _targetPos = new Vector3(_targetPos.x, _targetPos.y, _positionToFollow.z);
            }
        }
        else
        {
            _targetPos = _positionToFollow;
        }
        _targetPos += offset;
        transform.position = Vector3.SmoothDamp(transform.position, _targetPos, ref _velocity, _smoothTime);
        if(Vector3.Distance(transform.position,_targetPos)<0.01f) _isFollowing=false;
    }
    /// <summary>
    /// Sets camera position omitting the dampening effect.
    /// </summary>
    /// <param name="pos"></param>
    public void SetPositionToFollowRaw(Vector3 pos)
    {
        _positionToFollow = pos;
        if (_checkForBorders)
        {
            if ( _positionToFollow.x < leftScreenBorder.position.x)
            {
                _followOnXAxis = false;
                _positionToFollow = new Vector3(leftScreenBorder.position.x , _positionToFollow.y);
            }
            else
            {
                CheckIfPlayerIsOnRightScreenBorder();
            }

            if (_positionToFollow.y < lowerScreenBorder.position.y)
            {
                _followOnYAxis = false;
                _positionToFollow = new Vector3(_positionToFollow.x, lowerScreenBorder.position.y, _positionToFollow.z);

            }
            else
            {
                CheckIfPlayerIsOnUpperScreenBorder();
            }
        }
        _targetPos = _positionToFollow;
        _targetPos += offset;
        transform.position = _targetPos;
    }
    public void SetPositionToFollow(Vector3 pos)
    {
        _positionToFollow = pos;
        if (_checkForBorders)
        {
            if (leftScreenBorder != null && _positionToFollow.x  < leftScreenBorder.position.x)
            {
                _followOnXAxis = false;
                _positionToFollow = new Vector3(leftScreenBorder.position.x, _positionToFollow.y, _positionToFollow.z);
            }
            else
            {
                if(rightScreenBorder!=null) CheckIfPlayerIsOnRightScreenBorder();
            }

            if ( lowerScreenBorder!=null&& _positionToFollow.y < lowerScreenBorder.position.y)
            {
                _followOnYAxis = false;
                _positionToFollow = new Vector3(_positionToFollow.x, lowerScreenBorder.position.y, _positionToFollow.z);

            }
            else
            {
                if(upperScreenBorder!=null) CheckIfPlayerIsOnUpperScreenBorder();
            }
            if( backScreenBorder!=null && _positionToFollow.z <backScreenBorder.position.z)
            {
                _followOnZAxis = false;
                _positionToFollow = new Vector3(_positionToFollow.x, _positionToFollow.y, backScreenBorder.position.z);
            }
            else
            {
               if(forwardScreenBorder!=null) CheckIfPlayerIsOnForwardScreenBorder();
            }
        }

    }
    private void CheckIfPlayerIsOnRightScreenBorder()
    {
        if (_positionToFollow.x > rightScreenBorder.position.x)
        {
            _followOnXAxis = false;
            _positionToFollow = new Vector3(rightScreenBorder.position.x , _positionToFollow.y, _positionToFollow.z);
        }
        else
        {
            _followOnXAxis = true;
        }
    }
    private void CheckIfPlayerIsOnForwardScreenBorder()
    {
        if (_positionToFollow.z > forwardScreenBorder.position.z)
        {
            _followOnZAxis = false;
            _positionToFollow = new Vector3(_positionToFollow.x, _positionToFollow.y, forwardScreenBorder.position.z);
        }
        else
        {
            _followOnZAxis = true;
        }
    }
    private void CheckIfPlayerIsOnUpperScreenBorder()
    {
        if (_positionToFollow.y > upperScreenBorder.position.y)
        {
            _followOnYAxis = false;
            _positionToFollow = new Vector3(_positionToFollow.x, upperScreenBorder.position.y, _positionToFollow.y);
        }
        else
        {
            _followOnYAxis = true;
        }
    }
    private void OnDrawGizmos()
    {
        if (_debug)
        {
            if (_positionToFollow != null)
            {
                Gizmos.DrawWireSphere(_positionToFollow, _distanceToStartFollow);
            }
        }
    }
}
