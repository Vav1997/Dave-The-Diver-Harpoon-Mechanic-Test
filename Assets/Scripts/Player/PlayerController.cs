using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Header("Components")]
    [SerializeField] private PlayerVisual _playerVisual;
    [SerializeField] private WeaponController _weaponController;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _minMaxAimAngle;
    [SerializeField] private LookDirection _playerLookDirection;

    [SerializeField] private float _harpoonStrikeStartDelay;

    private StateMachine _stateMachine;
    private bool _isInitialized = false;
    private Vector3 _aimingDirectionClamped = Vector3.right;
  
    public LookDirection PlayerLookDirection => _playerLookDirection;
    public float MinMaxAimAngle => _minMaxAimAngle;
    public Vector3 AimingDirectionClamped => _aimingDirectionClamped;

    public event Action<LookDirection> OnPlayerLookDirectionChanged;
    public event Action<AimData> OnAimUpdate;
    
    public void Initialize() {

        //Initialize the state machine
        _stateMachine = new StateMachine();

        var walkState = new PlayerWalkState(this, _moveSpeed);
        var aimState = new PlayerAimState(this, _weaponController, OnAimStateEnterHandler, OnAimStateExitHandler, OnAimUpdateHandler);
        var shootHarpoonState = new PlayerHarpoonShootState(this, _weaponController, OnTargetHitHandler);

        //_stateMachine.AddTransition(walkState, aimState, new FuncPredicate(() => InputManager.Instance.IsRightMousePressed));
        At(walkState, aimState, new FuncPredicate(() => InputManager.Instance.IsRightMousePressed));
        At(aimState, walkState, new FuncPredicate(() => InputManager.Instance.IsRightMouseReleased));
        At(aimState, shootHarpoonState, new FuncPredicate(() => InputManager.Instance.IsLeftMousePressed));
        At(shootHarpoonState, walkState, new FuncPredicate(() => shootHarpoonState.HarpoonReturned == true && shootHarpoonState.IsBeingKnockedBack == false));

        _stateMachine.SetState(walkState);

        //Initialize components
        _playerVisual.Initialize(this);
        _weaponController.Initialize(this);

        _isInitialized = true;
    }

    private void Update() {
        if(!_isInitialized) {
            return;
        }

        _stateMachine.Update();
    }


    #region Aiming State Callbacks
    private void OnAimStateEnterHandler() {
        GameplayController.Instance.EnterAimStateHandler();
    }

    private void OnAimStateExitHandler() {
        GameplayController.Instance.ExitAimStateHandler();
    }

    private void OnAimUpdateHandler(AimData aimData) {
        _aimingDirectionClamped = aimData.ClampedDirection;
        OnAimUpdate?.Invoke(aimData);
    }

    #endregion AimingCallbacks

    #region Harpoon Shoot State Callbacks
    private void OnTargetHitHandler(TargetHitData targetHitData) {
        GameplayController.Instance.HarpoonHitTargetHandler(targetHitData.fishController);

        StartCoroutine(StartHarpoonStrikeWithDelay(targetHitData.fishController, targetHitData.fishDirectionFromPlayer));
    }

    private IEnumerator StartHarpoonStrikeWithDelay(FishController fishController, LookDirection fishDirectionFromPlayer) {
        yield return new WaitForSeconds(_harpoonStrikeStartDelay);
        GameplayController.Instance.HarpoonStrikeEnterHandler();
        HarpoonStrikeManager.Instance.Initialize(fishController, fishDirectionFromPlayer);
        HarpoonStrikeManager.Instance.OnFishCaught += OnFishCaughtHandler;
        HarpoonStrikeManager.Instance.OnFishLost += OnFishLostHandler;
    }


    public void OnHarpoonReturnedHandler() {
        GameplayController.Instance.OnHarpoonReturnedHandler();
    }

    #endregion Harpoon Shoot State Callbacks



    public void LookDirectionChangeHandler(LookDirection direction) {
        OnPlayerLookDirectionChanged?.Invoke(direction);
    }
    private void At(State from, State to, FuncPredicate condition) {
        _stateMachine.AddTransition(from, to, condition);
    }

    private void Any(State to, FuncPredicate condition) {
        _stateMachine.AddAnyTransition(to, condition);
    }

    private void OnFishCaughtHandler() {
        GameplayController.Instance.FishCatchingExitHandler();
        _weaponController.FishCaughtHandler();
    }
    private void OnFishLostHandler() {
        GameplayController.Instance.FishCatchingExitHandler();
        _weaponController.FishLostHandler();
    }
}


public enum LookDirection {
    Left,
    Right
}

