using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastFromMouse2D : MonoBehaviour
{
    [SerializeField] Camera _cam;
    [SerializeField] float _OverlapCircleRadius;
    [SerializeField] Transform _selectionSprite;
    private bool _isDragging=false;
    private Vector2 _dragStartPos;
    private Vector2 _selectionEndPos;
    private Vector2 _selectionStartPos;
    private List<IMouse2DRaycastSelectable> _selection = new List<IMouse2DRaycastSelectable>();
    private Collider2D[] _selectedColliders;
    // Start is called before the first frame update
    void Start()
    {
        if (_cam == null) _cam = Camera.main;
    }
    private void Update()
    {
        if(_isDragging)
        {
            _selection.Clear();
            _selectionEndPos = _cam.ScreenToWorldPoint(HelperClass.MousePos);

            _selectionSprite.localScale = _selectionEndPos - _selectionStartPos;

            _selectedColliders =Physics2D.OverlapAreaAll(_dragStartPos, _cam.ScreenToWorldPoint(HelperClass.MousePos));



            foreach (var collider in _selectedColliders)
            {
                IMouse2DRaycastSelectable selectable = collider.gameObject.GetComponent<IMouse2DRaycastSelectable>();
                if (selectable != null)
                {
                    _selection.Add(selectable);
                }
                
            }
        }
    }
    public Collider2D OverlapPoint(out Vector3 point, LayerMask mask)
    {
        Ray ray = _cam.ScreenPointToRay(HelperClass.MousePos);
        point = ray.origin;

        return Physics2D.OverlapPoint(ray.origin, mask);
    }
    public Collider2D[] OverlapCircleAll(out Vector3 point,LayerMask mask)
    {
        Ray ray = _cam.ScreenPointToRay(HelperClass.MousePos);
        point = ray.origin;

        return Physics2D.OverlapCircleAll(ray.origin, _OverlapCircleRadius, mask);
    }
    public void StartDrag()
    {
        _isDragging = true;
        _dragStartPos = _cam.ScreenToWorldPoint(HelperClass.MousePos);
        _selectionStartPos = _dragStartPos;
        _selectionSprite.position = _selectionStartPos;
        _selectionSprite.gameObject.SetActive(true);
        Logger.Log(_dragStartPos);
    }
    public void EndDrag()
    {
        _isDragging = false;
        _selectionSprite.gameObject.SetActive(false);
        foreach (IMouse2DRaycastSelectable selectable in _selection)
        {
            Logger.Log(selectable);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _OverlapCircleRadius);
    }
}
