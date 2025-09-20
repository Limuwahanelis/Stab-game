using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] Transform _objectToFollow;
    private Vector3 _offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _offset = transform.position - _objectToFollow.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _objectToFollow.position + _offset;
    }
    public void UpdateOffset()
    {
        _offset = transform.position - _objectToFollow.position;
    }
}
