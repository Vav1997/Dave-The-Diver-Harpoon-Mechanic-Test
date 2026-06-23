using System;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public static GameplayController Instance;

    [SerializeField] private PlayerController _playerController;
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private GlobalVolumeManager _globalVolumeManager;
    

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
    }


    private void Start() {
        InitializeGameplay();
    }

    private void InitializeGameplay() {
        _cameraManager.Initialize();
        _globalVolumeManager.Initialize();
        _playerController.Initialize();
    }

    public void EnterAimStateHandler() {
        _cameraManager.EnterAimFocusCamera();
        GlobalVolumeManager.Instance.EnterAimFocusState();
    }
    public void ExitAimStateHandler() {
        _cameraManager.SetRegularFOV();
        GlobalVolumeManager.Instance.ExitAimFocusState();
    }

    public void HarpoonHitTargetHandler(FishController fishController) {
        _cameraManager.EnterFishCatchFocus();
        _cameraManager.StartScreenShake();

        GameObject cameraMiddleTransform = new GameObject("CameraMiddlePoint");
        var stayBetweenTwo = cameraMiddleTransform.AddComponent<StayBetweenTwoPositions>();
        stayBetweenTwo.Initialize(_playerController.transform, fishController.transform);

        _cameraManager.SetTrackingTarget(cameraMiddleTransform.transform);
    }

    public void HarpoonStrikeEnterHandler() {
        _cameraManager.EnterHarpoonStrikeFocus();
    }

    public void OnHarpoonReturnedHandler() {
        
    }

    public void FishCatchingExitHandler() {
        _cameraManager.SetRegularFOV();
        _cameraManager.ExitScreenShake();
        _cameraManager.SetTrackingTarget(_playerController.transform);
    }

    public Transform GetPlayerTransform() {
        return _playerController.transform;
    }
}
