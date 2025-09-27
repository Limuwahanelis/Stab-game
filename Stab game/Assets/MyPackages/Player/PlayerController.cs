using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Debug"), SerializeField] bool _printState;
    public bool IsAlive => _isAlive;
    public PlayerState CurrentPlayerState => _currentPlayerState;
    public GameObject MainBody => _mainBody;
    [Header("Player")]
    [SerializeField] GameObject _mainBody;
    [SerializeField] AnimationManager _playerAnimationManager;
    [SerializeField] PlayerMovement2D _playerMovement;
    [SerializeField] PlayerRaycasts2D _raycasts2D;
    [SerializeField] AudioEventPlayer _playerAudioEventPlayer;
    [SerializeField] Legs2DIK _legsIk;
    [SerializeField] PlayerMovement2DIK _playerMovementIk;
    //[SerializeField] PlayerChecks _playerChecks;
    [SerializeField] PlayerHealthSystem _playerHealthSystem;
    private PlayerState _currentPlayerState;
    private PlayerContext _context;
    private Dictionary<Type, PlayerState> playerStates = new Dictionary<Type, PlayerState>();
    private bool _isAlive = true;
    [SerializeField,HideInInspector] private string _initialStateType;
    void Start()
    {

        Initalize();
    }
    protected void Initalize()
    {
       
        List<Type> states = AppDomain.CurrentDomain.GetAssemblies().SelectMany(domainAssembly => domainAssembly.GetTypes())
            .Where(type => typeof(PlayerState).IsAssignableFrom(type) && !type.IsAbstract).ToArray().ToList();

        _context = new PlayerContext
        {
            ChangePlayerState = ChangeState,
            animationManager = _playerAnimationManager,
            playerMovement = _playerMovement,
            WaitAndPerformFunction = WaitAndExecuteFunction,
            WaitFrameAndPerformFunction = WaitFrameAndExecuteFunction,
            audioEventPlayer = _playerAudioEventPlayer,
            coroutineHolder = this,
            playerRaycasts= _raycasts2D,
            ik = _legsIk,
            playerMovementIk = _playerMovementIk,
            //checks = _playerChecks,
            //combat = _playerCombat,
            //collisions = _playerCollisions,
        };

        PlayerState.GetState getState = GetState;
        foreach (Type state in states)
        {
            playerStates.Add(state, (PlayerState)Activator.CreateInstance(state, getState));
        }
        // Set Startitng state
        Logger.Log(Type.GetType(_initialStateType));
         PlayerState newState = GetState(Type.GetType(_initialStateType));
         newState.SetUpState(_context);
         _currentPlayerState = newState;
        Logger.Log(newState.GetType());
    }
    public PlayerState GetState(Type state)
    {
        return playerStates[state];
    }
    void Update()
    {
        _currentPlayerState.Update();
    }
    private void FixedUpdate()
    {
        _currentPlayerState.FixedUpdate();
    }
    public void ChangeState(PlayerState newState)
    {
        if (_printState) Logger.Log(newState.GetType());
        _currentPlayerState.InterruptState();
        _currentPlayerState = newState;
    }

    public void PushPlayer(PushInfo psuhInfo)
    {
        _currentPlayerState.Push();
    }
    public Coroutine WaitAndExecuteFunction(float timeToWait, Action function)
    {
        return StartCoroutine(HelperClass.DelayedFunction(timeToWait, function));
    }
    public Coroutine WaitFrameAndExecuteFunction(Action function)
    {
        return StartCoroutine(HelperClass.WaitFrame(function));
    }

    private void OnDestroy()
    {

    }
}
