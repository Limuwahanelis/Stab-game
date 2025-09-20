using System.Threading;
using Unity.Mathematics;
# if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerCameraMouse : MonoBehaviour
{
    [SerializeField,Header("Debug")] bool _debug;
    [SerializeField,Header("Common")] GameObject _focalPoint;
    [SerializeField] bool _useMouse=true;
    [Header("Ranges")]
    [SerializeField, MinMaxRange(0, 50)] RangedFloat _zoomDistance;
    [SerializeField, MinMaxRange(-180,180 )] RangedFloat _pitchLevels;
    [Header("Rotations")]
    [SerializeField] float mouseRotationSpeed = 0.2f;
    [SerializeField] float keyboardRotationSpeed = 2f;
    [Header("Scroll")]
    [SerializeField] float scrollSpeed = 0.2f;
    [Header("Smoothness")]
    [SerializeField] float _followRange=0.5f;
    [SerializeField] float smoothTime = 1f;
    private float scrollDir;
    private Vector3 offset;
    float screenX = Screen.width;
    float screenY = Screen.height;

    float yaw = 0.0f;
    float pitch = 0.0f;


    private Vector3 _targetPos;
    private Vector3 _currentTargetPos;
    private Vector3 _velocity = Vector3.zero;

    bool _isFollowing = false;
    bool locked = true;

    private Quaternion _rotation;
    float scrollY = 0;
    float startingDistanceFromFocalPoint = 0;
    float distanceFromFocalPoint = 0;
    // Start is called before the first frame update
    void Start()
    {
        startingDistanceFromFocalPoint = (transform.position - _focalPoint.transform.position).magnitude;
        scrollY = -startingDistanceFromFocalPoint+1;
        offset = transform.position - _focalPoint.transform.position;
        offset = new Vector3(0,0, -1);
        _targetPos = _focalPoint.transform.position;
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }
    protected void AlignWithFocalPoint()
    {
        transform.LookAt(_focalPoint.transform);
        Vector3 vec = transform.position - _focalPoint.transform.position;
        float dist = (transform.position - _focalPoint.transform.position).magnitude;
        transform.position = _focalPoint.transform.position + vec.normalized * _zoomDistance.maxValue;

    }
    // Update is called once per frame
    void Update()
    {
        float mouseX = HelperClass.MousePos.x;
        float mouseY = HelperClass.MousePos.y;
        if (mouseX < 0 || mouseX > screenX || mouseY < 0 || mouseY > screenY)
            return;
        if (Vector3.Distance(_focalPoint.transform.position, _targetPos) > _followRange)
        {
            _isFollowing = true;
            _currentTargetPos = _focalPoint.transform.position;
        }
       
    }
    private void FixedUpdate()
    {
        Rotate();
    }
    public void RotateKeyboardX(float value)
    {
        if (_useMouse) return;
        yaw += keyboardRotationSpeed * value;
        if (yaw >= 360) yaw = 0;
        if (yaw <= -360) yaw = 0;
    }
    public void RotateKeyboardY(float value)
    {
        if (_useMouse) return;
        pitch += keyboardRotationSpeed * value;
        if (pitch >= 360) pitch = 0;
        if (pitch <= -360) pitch = 0;
    }
    public void RotateMouse(float valueX,float valueY)
    {
        if (!_useMouse) return;
        if (HelperClass.MousePos.x < 0 || HelperClass.MousePos.x > screenX) return;
        if (HelperClass.MousePos.y < 0 || HelperClass.MousePos.y > screenY) return;
        yaw += mouseRotationSpeed * valueX;
        pitch += mouseRotationSpeed * valueY;
        if (pitch >= 360) pitch = 0;
        if (yaw >= 360) yaw = 0;
        if (pitch <= -360) pitch = 0;
        if (yaw <= -360) yaw = 0;
    }
    public void ResetPivot()
    {
        _focalPoint.transform.rotation = Quaternion.identity;
        yaw = 0;
    }
    public void Scroll(float value)
    {
        if (!_useMouse) return;
        if (HelperClass.MousePos.x < 0 || HelperClass.MousePos.x > screenX) return;
        if (HelperClass.MousePos.y < 0 || HelperClass.MousePos.y > screenY) return;
        scrollDir = math.clamp(value, -1, 1);
        scrollY += scrollSpeed * scrollDir*Time.deltaTime;
        scrollY = math.clamp(scrollY, -_zoomDistance.maxValue + 1, -_zoomDistance .minValue+ 1);
        distanceFromFocalPoint = -startingDistanceFromFocalPoint + scrollY;

    }
    private void Rotate()
    {
        if (pitch >= _pitchLevels.maxValue)
            pitch = _pitchLevels.maxValue;
        if (pitch <= _pitchLevels.minValue)
            pitch = _pitchLevels.minValue;
        transform.eulerAngles = new Vector3(pitch, yaw);
        _rotation = Quaternion.Euler(pitch, yaw, 0);
        if(_isFollowing)
        {
            _targetPos=Vector3.SmoothDamp(_targetPos, _currentTargetPos, ref _velocity, smoothTime);
            if (Vector3.Distance(_currentTargetPos, _targetPos) < 0.01f) _isFollowing = false;
        }
        transform.position = _targetPos+ (_rotation * offset) + transform.forward * scrollY;
    }
    private void OnDrawGizmos()
    {
        if (!enabled) return;
        if (!_debug) return;
        if(_focalPoint!=null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_focalPoint.transform.position, _zoomDistance.minValue);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_focalPoint.transform.position, _zoomDistance.maxValue);
        }
    }
    private void OnDestroy()
    {

    }
#if UNITY_EDITOR
    [CustomEditor(typeof(PlayerCameraMouse))]
    public class PlayerCameraMouseEditor:Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            if (GUILayout.Button("Align with focal point"))
            {
                (target as PlayerCameraMouse).AlignWithFocalPoint();
            }
            base.OnInspectorGUI();

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
