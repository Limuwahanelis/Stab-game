using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemyController : NPCController
{
    [Header("Debug"), SerializeField] bool _printState;
    [SerializeField] protected bool _debug;
    
    public GameObject MainBody => _mainBody;
    [Header("Enemy common")]
    [SerializeField] protected bool _initializeOnStart=true;
    [SerializeField] protected AnimationManager _enemyAnimationManager;
    [SerializeField] protected Transform _playerTransform;
    [SerializeField] protected Rigidbody2D _playerRB;
    [SerializeField] protected GameObject _mainBody;
    [SerializeField] protected StandardHealthBar _healthBar;
    [SerializeField] protected HealthSystem _healthSystem;

    protected virtual void Awake()
    {
        if (_playerTransform == null) _playerTransform = FindFirstObjectByType<PlayerController>().MainBody.transform;
        
    }
    protected virtual void Start()
    {
        if (_initializeOnStart) Initialize();
    }
    protected virtual void Initialize()
    {
        List<Type> states = AppDomain.CurrentDomain.GetAssemblies().SelectMany(domainAssembly => domainAssembly.GetTypes())
.Where(type => typeof(EnemyState).IsAssignableFrom(type) && !type.IsAbstract && type.ToString().Contains("NewEnemy")).ToArray().ToList();


        EnemyState.GetState getState = GetState;
        foreach (Type state in states)
        {
            _npcStates.Add(state, (EnemyState)Activator.CreateInstance(state, getState));
        }


    }
    public EnemyState GetState(Type state)
    {
        return _npcStates[state] as EnemyState;
    }
    public virtual void Update()
    {
        if (PauseSettings.IsGamePaused) return;
        _currentNPCState.Update();
    }
    public virtual void FixedUpdate()
    {
        if (PauseSettings.IsGamePaused) return;
        _currentNPCState.FixedUpdate();
    }
    public void ChangeState(NPCState newState)
    {
        if (_printState) Logger.Log(newState.GetType());
        _currentNPCState.InterruptState();
        _currentNPCState = newState;
    }
    public Coroutine WaitFrameAndExecuteFunction(Action function)
    {
        return StartCoroutine(WaitFrame(function));
    }
    public IEnumerator WaitFrame(Action function)
    {
        yield return new WaitForNextFrameUnit();
        function();
    }
}
