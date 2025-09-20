using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerDetection : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] bool _debug;
#endif
    public UnityEvent<Collider> OnColliderDetected;
    public UnityEvent<Collider> OnColliderLeft;
    List<TriggerDetectable> _detectables = new List<TriggerDetectable>();
    List<Collider> _collidersInStay = new List<Collider>();
    private Rigidbody _rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
#if UNITY_EDITOR
        Logger.Log("Detected: "  + other.gameObject.name);
#endif
        TriggerDetectable detectable = null;

        detectable = other.GetComponent<TriggerDetectable>();

        if (detectable != null && !_detectables.Contains(detectable))
        {
            _detectables.Add(detectable);
            detectable.OnDisabled.AddListener(DetectableDisable);
        }
        OnColliderDetected?.Invoke(other);
    }
    private void OnTriggerStay(Collider other)
    {
        
        TriggerDetectable detectable = null;

        detectable = other.GetComponent<TriggerDetectable>();

        if (detectable != null && !_detectables.Contains(detectable))
        {
            _detectables.Add(detectable);
            detectable.OnDisabled.AddListener(DetectableDisable);
        }
        if(!_collidersInStay.Contains(other))
        {
#if UNITY_EDITOR
            Logger.Log("Detected: "+other.gameObject.name);
#endif
            _collidersInStay.Add(other);
            OnColliderDetected?.Invoke(other);
        }
  
    }

    private void OnTriggerExit(Collider other)
    {
#if UNITY_EDITOR
        Logger.Log("Left " + other.gameObject.name);
#endif
        TriggerDetectable detectable = other.GetComponent<TriggerDetectable>();
        if (detectable!=null && _detectables.Contains(detectable))
        {
            _detectables.Remove(detectable);
            detectable.OnDisabled.RemoveListener(DetectableDisable);
        }
        OnColliderLeft?.Invoke(other);
    }

    private void DetectableDisable(TriggerDetectable detectable)
    {
        _detectables.Remove(detectable);
        Collider col = detectable.GetComponent<Collider>();
        if (_collidersInStay.Contains(col)) _collidersInStay.Remove(col);
#if UNITY_EDITOR
        Logger.Log("Left " + col.gameObject.name);
#endif
        OnColliderLeft?.Invoke(col);
    }
    private void OnDestroy()
    {
        foreach (var detectable in _detectables)
        {
            detectable.OnDisabled.RemoveListener(DetectableDisable);
        }
    }
}
