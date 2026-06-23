using System;
using UnityEngine;

public class PlayerAimState : PlayerStateMachineState {

    private WeaponController _weaponController;
    private Action OnEnter;
    private Action OnExit;
    private event Action<AimData> OnAimUpdate;
    private Camera _mainCam;
    public PlayerAimState(PlayerController controller,
        WeaponController weaponController,
        Action OnEnter,
        Action OnExit,
        Action<AimData> OnAimUpdate
        ) : base(controller) {
        _weaponController = weaponController;
        this.OnEnter = OnEnter;
        this.OnExit = OnExit;
        this.OnAimUpdate = OnAimUpdate;
    }

    public override void EnterState() {
        _weaponController.StartAiming();
        OnEnter?.Invoke();
        _mainCam = Camera.main;
    }

    public override void UpdateState() {
        Vector3 playerScreenPos = _mainCam.WorldToScreenPoint(_controller.transform.position);
        bool isFacingRight = Input.mousePosition.x > playerScreenPos.x;

        _controller.LookDirectionChangeHandler(isFacingRight ? LookDirection.Right : LookDirection.Left);
        

        var harpoonScreenPosition = _mainCam.WorldToScreenPoint(_weaponController.HarpoonSocketTransform.position);
        Vector3 aimingDirection = CalculateAimingDireciton(harpoonScreenPosition, Input.mousePosition, _controller.MinMaxAimAngle, isFacingRight);
        float aimingAngle = CalculateAimingAngle(harpoonScreenPosition, Input.mousePosition, _controller.MinMaxAimAngle, isFacingRight);

        OnAimUpdate?.Invoke(new AimData {
            ClampedAngle = aimingAngle,
            ClampedDirection = aimingDirection,
            IsFacingRight = isFacingRight
        });
    }

    public override void ExitState() {
        _weaponController.ExitAiming();
        OnExit?.Invoke();
    }

    public Vector3 CalculateAimingDireciton(Vector3 startingScreenPosition, Vector3 mouseScreenPosition, float aimLimitAngle, bool isFacingRight) {

        Vector3 direction = (mouseScreenPosition - startingScreenPosition).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if(angle < 0) {
            angle += 360f;
        }

        float clampedAngle = isFacingRight
            ? Mathf.Clamp(angle > 180f ? angle - 360 : angle, -aimLimitAngle, aimLimitAngle)
            : Mathf.Clamp(angle, 180f - aimLimitAngle, 180f + aimLimitAngle);

        return new Vector3(Mathf.Cos(clampedAngle * Mathf.Deg2Rad), Mathf.Sin(clampedAngle * Mathf.Deg2Rad), 0f);
    }

    public float CalculateAimingAngle(Vector3 startingScreenPosition, Vector3 mouseScreenPosition, float aimLimitAngle, bool isFacingRight) {
        Vector3 direction = (mouseScreenPosition - startingScreenPosition).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if(angle < 0) {
            angle += 360f;
        }

        return isFacingRight
            ? Mathf.Clamp(angle > 180f ? angle - 360 : angle, -aimLimitAngle, aimLimitAngle)
            : Mathf.Clamp(angle, 180f - aimLimitAngle, 180f + aimLimitAngle);
    }

    public override void OnMyTriggerEnter(Collider other) {
        
    }
}
