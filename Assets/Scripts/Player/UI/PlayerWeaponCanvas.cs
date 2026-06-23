using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerWeaponCanvas : MonoBehaviour
{
    [SerializeField] private RectTransform _harpoonPanel;
    [SerializeField] private RectTransform _arrowPointer;
    [SerializeField] private RectTransform _aimImage;
    [SerializeField] private float _offsetFromCenter;
    private WeaponController _controller;

    public void Initialize(WeaponController controller) {
        _controller = controller;
        _controller.OnEnterAim += OnEnterAimHandler;
        _controller.OnExitAim += OnExitAimHandler;
        _controller.OnAimUpdate += OnAimUpdateHandler;
        _aimImage.position = new Vector3(transform.position.x + _offsetFromCenter, 0, 0);
        Hide();
    }


    private void OnAimUpdateHandler(AimData aimData) {

        float offset = aimData.IsFacingRight ? _offsetFromCenter : -_offsetFromCenter;
        int scaleMultiplier = aimData.IsFacingRight ? 1 : -1;
        _aimImage.localPosition = new Vector3(transform.localPosition.x + offset, 0, 0);
        _aimImage.localScale = new Vector3(Mathf.Abs(_aimImage.localScale.x) * scaleMultiplier, _aimImage.localScale.y, _aimImage.localScale.z);

        _arrowPointer.position = _controller.HarpoonSocketTransform.position;

        _arrowPointer.position = _controller.HarpoonSocketTransform.position + aimData.ClampedDirection * Mathf.Abs(offset);
        _arrowPointer.rotation = Quaternion.Euler(0, 0, aimData.ClampedAngle);
    }

    private void OnExitAimHandler() {
        Hide();
    }

    private void OnEnterAimHandler() {
        
        Show();
    }


    private void Show() {
        _harpoonPanel.gameObject.SetActive(true);
    }

    private void Hide() {
        _harpoonPanel.gameObject.SetActive(false);
    }
}
