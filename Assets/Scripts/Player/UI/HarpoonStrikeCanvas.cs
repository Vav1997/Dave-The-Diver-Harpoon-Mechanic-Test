using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HarpoonStrikeCanvas : MonoBehaviour
{
    [SerializeField] private Image _fillbarFG;
    
    [SerializeField] private Canvas _canvas;
    
    private HarpoonStrikeManager _harpoonStrikeManager;
    

    private void Awake() {
        Hide();
    }
    public void Initialize(HarpoonStrikeManager harpoonStrikeManager) {
        _harpoonStrikeManager = harpoonStrikeManager;
        _harpoonStrikeManager.OnFishCaught += OnFishCaughtHandler;
        _harpoonStrikeManager.OnFishLost += OnFishLostHandler;
        _harpoonStrikeManager.OnProgressNormalizedChanged += OnProgressNormalizedChangedHandler;
        var lookDirection = _harpoonStrikeManager.GetFishDirectionFromPlayer();

        _canvas.transform.localScale = lookDirection == LookDirection.Right ? Vector3.one :
            new Vector3(- 1, _canvas.transform.localScale.y, _canvas.transform.localScale.z);

        Show();
    }

    private void OnProgressNormalizedChangedHandler(float progressNormalized) {
        _fillbarFG.fillAmount = progressNormalized;
    }

    private void OnFishLostHandler() {
        Hide();
    }

    private void OnFishCaughtHandler() {
        Hide();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void Show() {
        gameObject.SetActive(true);
    }
}

