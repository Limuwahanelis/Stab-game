using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DraggableWindow : MonoBehaviour
{
    private Vector3 _offset;
    public void OnBeginDrag(BaseEventData data)
    {
        _offset = (Vector2)transform.position-((PointerEventData)data).position;
    }
    public void OnDrag(BaseEventData data)
    {
        transform.position = (Vector3)((PointerEventData)data).position + _offset;
    }
}
