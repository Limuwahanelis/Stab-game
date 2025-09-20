using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    [SerializeField] Camera _cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _cam.ScreenToWorldPoint(HelperClass.MousePos);
    }
}
