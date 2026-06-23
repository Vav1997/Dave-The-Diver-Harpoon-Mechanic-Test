using System;
using System.Diagnostics;
using UnityEngine;
public class PlayerHarpoonShootState : PlayerStateMachineState {

    private WeaponController _weaponController;
    public bool IsBeingKnockedBack { get; private set; }
    public bool HarpoonReturned {  get; private set; }

    private Vector3 _knockbackVelocity;
    private float _knockbackDamping = 4f;

    private Action<TargetHitData> OnHitTarget;
    public PlayerHarpoonShootState(PlayerController controller, WeaponController weaponController, Action<TargetHitData> OnHitTarget) : base(controller) {
        _weaponController = weaponController;
        this.OnHitTarget = OnHitTarget;
    }

    public override void EnterState() {
        _weaponController.Shoot(_controller.AimingDirectionClamped);
        _weaponController.OnHarpoonReturned += OnHarpoonReturnedHandler;
        _weaponController.OnHitTarget += OnHitTargetHandler;

        _knockbackVelocity = -_controller.AimingDirectionClamped * 5f;
        IsBeingKnockedBack = true;
    }

    private void OnHitTargetHandler(TargetHitData targetHitData) {
        OnHitTarget?.Invoke(targetHitData);
    }

    private void OnHarpoonReturnedHandler() {
        _controller.OnHarpoonReturnedHandler();
        HarpoonReturned = true;
    }

    public override void ExitState() {
        _weaponController.OnHarpoonReturned -= OnHarpoonReturnedHandler;
        _weaponController.OnHitTarget -= OnHitTargetHandler;
        HarpoonReturned = false;
    }

    public override void UpdateState() {
        if(IsBeingKnockedBack) {
            _controller.transform.position += (Vector3)_knockbackVelocity * Time.deltaTime;
            _knockbackVelocity = Vector2.Lerp(_knockbackVelocity, Vector2.zero, _knockbackDamping * Time.deltaTime);

            if(_knockbackVelocity.sqrMagnitude < 0.01f) {
                IsBeingKnockedBack = false;
                _knockbackVelocity = Vector2.zero;
            }
        }
    }

    public override void OnMyTriggerEnter(Collider other) {
    }
}