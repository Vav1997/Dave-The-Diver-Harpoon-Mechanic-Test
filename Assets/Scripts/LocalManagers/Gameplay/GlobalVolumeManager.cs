using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlobalVolumeManager : MonoBehaviour
{
    public static GlobalVolumeManager Instance;

    [SerializeField] private Volume _mainVolume;

    [Header("Vignette Settings")]
    [SerializeField] private float _aimStateVignetteIntencity;


    [Header("Chromatic abberation Settings")]
    [SerializeField] private float _aimStateChromaticAbberationIntencity;

    [SerializeField] private float _aimStateTransitionEnterTime;
    [SerializeField] private float _aimStateTransitionExitTime;

    private Vignette _vignette;
    private ChromaticAberration _chromaticAberration;
    private float _startingVignetteIntencity;
    private float _startingchromaticAberrationIntencity;

    private Coroutine _vignetteLerpCor;
    private Coroutine _chromaticAberrationLerpCor;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
    }

    public void Initialize() {
        if(_mainVolume.profile.TryGet(out Vignette vignette)) {
            _vignette = vignette;
            _startingVignetteIntencity = _vignette.intensity.value;
        } else {
            Debug.LogError("No Vignette was found");
        }

        if(_mainVolume.profile.TryGet(out ChromaticAberration chromaticAberration)) {
            _chromaticAberration = chromaticAberration;
            _startingchromaticAberrationIntencity = _chromaticAberration.intensity.value;
        }
        else {
            Debug.LogError("No ChromaticAberration was found");
        }
    }
    public void EnterAimFocusState() {
        if(_vignetteLerpCor != null) {
            StopCoroutine(_vignetteLerpCor);
        }
        _vignetteLerpCor = StartCoroutine(LerpVignette(_aimStateVignetteIntencity, _aimStateTransitionEnterTime));
        _chromaticAberrationLerpCor = StartCoroutine(LerpChromaticAbberation(_aimStateChromaticAbberationIntencity, _aimStateTransitionEnterTime));
    }


    public void ExitAimFocusState() {
        if(_vignetteLerpCor != null) {
            StopCoroutine(_vignetteLerpCor);
        }
        _vignetteLerpCor = StartCoroutine(LerpVignette(_startingVignetteIntencity, _aimStateTransitionExitTime));
        _chromaticAberrationLerpCor = StartCoroutine(LerpChromaticAbberation(_startingchromaticAberrationIntencity, _aimStateTransitionExitTime));
    }

    private IEnumerator LerpVignette(float targetIntencity, float lerpTime) {
        float startingIntencity = _vignette.intensity.value;
        float elapsedTime = 0;

        while(elapsedTime < lerpTime) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / lerpTime;
            _vignette.intensity.value = Mathf.Lerp(startingIntencity, targetIntencity, t);
            yield return null;
        }

        _vignette.intensity.value = targetIntencity;
    }

    private IEnumerator LerpChromaticAbberation(float targetIntencity, float lerpTime) {
        float startingIntencity = _chromaticAberration.intensity.value;
        float elapsedTime = 0;

        while(elapsedTime < lerpTime) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / lerpTime;
            _chromaticAberration.intensity.value = Mathf.Lerp(startingIntencity, targetIntencity, t);
            yield return null;
        }

        _chromaticAberration.intensity.value = targetIntencity;
    }
}
