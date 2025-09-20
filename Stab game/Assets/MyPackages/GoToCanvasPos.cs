using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Events;

public class GoToCanvasPos : MonoBehaviour
{
    public UnityEvent<GoToCanvasPos> OnArrived;
    [SerializeField] RenderMode _canvasType;
    [SerializeField] RectTransform _targetTransform;
    [SerializeField] RectTransform _canvasTrasform;
    [SerializeField] bool _useCameraCipPlane;
    [SerializeField] float _zPos;
    [SerializeField] float _speed;
    [SerializeField] float _epsilon;
    private Vector2 _targetScreenPos;
    private Vector3 _modifedScreenPos;
    private Vector3 _targetWorldPos;
    private Vector3 _moveVector;
    bool _followObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    public void StartFollow()
    {
        _followObject = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (_followObject)
        {
            if (_canvasType == RenderMode.ScreenSpaceOverlay)
            {
                _targetScreenPos = RectTransformUtility.WorldToScreenPoint(null, _targetTransform.position);
                _modifedScreenPos = new Vector3(_targetScreenPos.x, _targetScreenPos.y, _useCameraCipPlane ? Camera.main.nearClipPlane : _zPos);
                _targetWorldPos = Camera.main.ScreenToWorldPoint(_modifedScreenPos);

                _moveVector = (_targetWorldPos - transform.position).normalized;
                if (Vector3.Distance(_targetWorldPos, transform.position) > _epsilon)
                {
                    transform.position += _moveVector * Time.deltaTime * _speed;
                }
                else
                {
                    OnArrived?.Invoke(this);
                    _followObject = false;
                }

            }
            else if (_canvasType == RenderMode.ScreenSpaceCamera)
            {
                _targetWorldPos = _targetTransform.position;
                _targetWorldPos.z= _zPos;
                _moveVector = (_targetWorldPos - transform.position).normalized;
                if (Vector3.Distance(_targetWorldPos, transform.position) > _epsilon)
                {
                    transform.position += _moveVector * Time.deltaTime * _speed;
                }
                else
                {
                    OnArrived?.Invoke(this);
                    _followObject = false;
                }
            }

        }
    }
}
