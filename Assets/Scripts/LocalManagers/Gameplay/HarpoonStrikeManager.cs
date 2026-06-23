using System;
using UnityEngine;

public class HarpoonStrikeManager : MonoBehaviour
{
    public static HarpoonStrikeManager Instance;

    [SerializeField] private HarpoonStrikeCanvas _harpoonStrikeCanvas;

    [SerializeField] private float _maxTimeToCatchFish;
    [SerializeField] private float _progressMax = 10;
    [SerializeField, Range(0, 1)] private float _startProgressFillPercentage;
    [SerializeField] private float _progressDecreaseSpeed;
    [SerializeField] private float _actionProgressIncreaseAmount;

    private float _currentProgress;
    private float _catchFishTimer;
    private State _state;
    private LookDirection _fishDirectionFromPlayer;
    private FishController _fishController;

    public Action OnFishCaught;
    public Action OnFishLost;
    public Action<float> OnProgressNormalizedChanged;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
    }

    public void Initialize(FishController fishController, LookDirection fishDirectionFromPlayer) {
        _fishController = fishController;
        _fishDirectionFromPlayer = fishDirectionFromPlayer;
        _currentProgress = _progressMax * _startProgressFillPercentage;
        _state = State.Catching;
        _catchFishTimer = _maxTimeToCatchFish;
        _harpoonStrikeCanvas.Initialize(this);
    }


    public void Update() {
        if(_state == State.Catching) {
            if(Input.GetKeyDown(KeyCode.Space)) {
                ActionButonHandler();
            }

            _currentProgress -= Time.deltaTime * _progressDecreaseSpeed;
            OnProgressNormalizedChanged?.Invoke(_currentProgress / _progressMax);
            if(_currentProgress <= 0) {
                
                OnFishLost?.Invoke();
                _fishController.ReleaseFromHarpoon();
                FinishCatchingHandler();
            }

            _catchFishTimer -= Time.deltaTime;
            if(_catchFishTimer <= 0) {
                OnFishLost?.Invoke();
                _fishController.ReleaseFromHarpoon();
                FinishCatchingHandler();
            }
        }
    }

    public void FinishCatchingHandler() {
        _state = State.None;
        _currentProgress = _progressMax * _startProgressFillPercentage;
    }

    public void ActionButonHandler() {
        _currentProgress += _actionProgressIncreaseAmount;
        OnProgressNormalizedChanged?.Invoke(_currentProgress / _progressMax);
        if(_currentProgress >= _progressMax) {
            OnFishCaught?.Invoke();
            FinishCatchingHandler();
        }
    }


    public LookDirection GetFishDirectionFromPlayer() {
        return _fishDirectionFromPlayer;
    }

    public enum State {
        None,
        Catching,
    }

}
