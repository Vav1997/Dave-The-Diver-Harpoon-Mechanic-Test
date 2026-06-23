using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] private CinemachineCamera _mainCinemachineCam;
    [SerializeField] private Camera _uiCam;

    [SerializeField] private float _focusedFOVAmount;
    [SerializeField] private float _focusedEnterFOVTime;
    [SerializeField] private float _focusedExitFOVTime;

    [SerializeField] private float _fishCatchFocusFOVAmount;
    [SerializeField] private float _fishCatchEnterFOVTime;

    [SerializeField] private float _harpoonStrikeFOVAmount;
    [SerializeField] private float _harpoonStrikeEnterFOVTime;

    [SerializeField] private float _shakeAmplitude;

    private float _startingFOVAmount;
    private Coroutine _cameraFOVLerpCor;
    private Coroutine _shakeAmplitudeLerpCor;

    private CinemachineBasicMultiChannelPerlin _multiChannelPerlin;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
    }

    public void Initialize() {
        _startingFOVAmount = _mainCinemachineCam.Lens.FieldOfView;
        _multiChannelPerlin = _mainCinemachineCam.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void EnterAimFocusCamera() {
        if(_cameraFOVLerpCor != null) {
            StopCoroutine(_cameraFOVLerpCor);
        }

        _cameraFOVLerpCor = StartCoroutine(LerpCameraFOV(_mainCinemachineCam, _focusedFOVAmount, _focusedEnterFOVTime));

    }


    public void SetRegularFOV() {
        if(_cameraFOVLerpCor != null) {
            StopCoroutine(_cameraFOVLerpCor);
        }
        _cameraFOVLerpCor = StartCoroutine(LerpCameraFOV(_mainCinemachineCam, _startingFOVAmount, _focusedExitFOVTime));
    }

    public void EnterFishCatchFocus() {
        if(_cameraFOVLerpCor != null) {
            StopCoroutine(_cameraFOVLerpCor);
        }

        _cameraFOVLerpCor = StartCoroutine(LerpCameraFOV(_mainCinemachineCam, _fishCatchFocusFOVAmount, _fishCatchEnterFOVTime));
    }

    public void EnterHarpoonStrikeFocus() {
        if(_cameraFOVLerpCor != null) {
            StopCoroutine(_cameraFOVLerpCor);
        }

        _cameraFOVLerpCor = StartCoroutine(LerpCameraFOV(_mainCinemachineCam, _harpoonStrikeFOVAmount, _harpoonStrikeEnterFOVTime));
    }

    public void StartScreenShake() {
        if(_shakeAmplitudeLerpCor != null) {
            StopCoroutine(_shakeAmplitudeLerpCor);
        }
        _shakeAmplitudeLerpCor = StartCoroutine(LerpShakeAmplitude(_shakeAmplitude, 0));
    }

    public void ExitScreenShake() {
        if(_shakeAmplitudeLerpCor != null) {
            StopCoroutine(_shakeAmplitudeLerpCor);
        }
        _shakeAmplitudeLerpCor = StartCoroutine(LerpShakeAmplitude(0, 0f));
    }

    public void SetTrackingTarget(Transform target) {
        _mainCinemachineCam.Follow = target;
    }

    private IEnumerator LerpCameraFOV(CinemachineCamera camera, float targetFOV, float lerpTime) {
        float startingFOV = camera.Lens.FieldOfView;
        float elapsedTime = 0;

        while(elapsedTime < lerpTime) {
            elapsedTime += Time.deltaTime;
            float elapsedPercentage = elapsedTime / lerpTime;
            _uiCam.fieldOfView = targetFOV;
            camera.Lens.FieldOfView = Mathf.Lerp(startingFOV, targetFOV, elapsedPercentage);
            yield return null;
        }

        camera.Lens.FieldOfView = targetFOV;
        _uiCam.fieldOfView = targetFOV;
    }

    private IEnumerator LerpShakeAmplitude(float targetAmplitude, float lerpTime) {
        float startingAmplitude = _multiChannelPerlin.AmplitudeGain;
        float elapsedTime = 0;

        while(elapsedTime < lerpTime) {
            elapsedTime += Time.deltaTime;
            float elapsedPercentage = elapsedTime / lerpTime;
            _multiChannelPerlin.AmplitudeGain = Mathf.Lerp(startingAmplitude, targetAmplitude, elapsedPercentage);
            yield return null;
        }

        _multiChannelPerlin.AmplitudeGain = targetAmplitude;
    }
}
