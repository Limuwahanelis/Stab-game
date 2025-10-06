using UnityEngine;

public class PlayerRaycasts2D : MonoBehaviour
{
    public RaycastHit2D GroundHit => _groundHit;
    [SerializeField] bool _debug;
    [Header("Layers")]
    [SerializeField] LayerMask _groundLayer;
    [Header("Rays")]
    [SerializeField] Transform _groundRayTran;
    [SerializeField] float _groundRayDistance;


    private bool _isOnGround;
    private RaycastHit2D _groundHit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _groundHit = Physics2D.Raycast(_groundRayTran.position, Vector2.down, _groundRayDistance, _groundLayer);
    }
    private void OnDrawGizmos()
    {
        if(_debug)
        {
            if(_groundRayTran)
            {
                Gizmos.DrawLine(_groundRayTran.position, _groundRayTran.position + (Vector3)Vector2.down * _groundRayDistance);
            }
        }
    }
}
