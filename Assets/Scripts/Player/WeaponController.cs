using System;
using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private PlayerWeaponCanvas _playerWeaponCanvas;

    [SerializeField] private VerletRope _ropePrefab;
    [SerializeField] private HarpoonProjectileController _projectilePrefab;
    [SerializeField] private float _minMaxAimAngle;
    [SerializeField] private Transform _harpoonSockedTransform;

    public float MinMaxAimAngle => _minMaxAimAngle;
    public Transform HarpoonSocketTransform => _harpoonSockedTransform;

    public event Action OnEnterAim;
    public event Action OnExitAim;
    public event Action<AimData> OnAimUpdate;
    public event Action OnHarpoonReturned;
    public event Action<TargetHitData> OnHitTarget;

    private bool _isAiming = false;
    private PlayerController _playerController;
    private HarpoonProjectileController _currentProjectile;
    private VerletRope _currentRope;


    public void Initialize(PlayerController playerController) {
        _playerController = playerController;
        _playerController.OnAimUpdate += OnAimUpdateHandler;
        _playerWeaponCanvas.Initialize(this);
    }

    private void Update() {
        if(!_isAiming) {
            return;
        }
    }

    private void OnAimUpdateHandler(AimData aimData) {
        OnAimUpdate?.Invoke(aimData);
    }

    public void StartAiming() {
        OnEnterAim?.Invoke();

    }

    public void ExitAiming() {
        OnExitAim?.Invoke();
    }

    public void Shoot(Vector3 direcitonNormalized) {
        ExitAiming();
        _currentProjectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);

        _currentProjectile.Initialize(new ProjectileData {
            flyingDistance = 7f,
            moveDirection = direcitonNormalized,
            moveSpeed = 50f,
            projectileSpawnTransform = _harpoonSockedTransform
        });

        _currentProjectile.OnReachMaxDistance += OnReachMaxDistanceHandler;
        _currentProjectile.OnReturnedBack += OnReturnedBackHandler;
        _currentProjectile.OnHitTarget += OnHitTargetHandler;

        _currentRope = Instantiate(_ropePrefab, transform.position, Quaternion.identity);
        _currentRope.SetStartEndPoints(_harpoonSockedTransform, _currentProjectile.GetRopeAttachTransform());
    }

    private void OnHitTargetHandler(TargetHitData targetHitData) {
        OnHitTarget?.Invoke(targetHitData);
    }

    private void OnReturnedBackHandler() {
        _currentProjectile.DestroySelf();
        _currentRope.DestroySelf();
        _currentProjectile = null;
        _currentRope = null;
        OnHarpoonReturned?.Invoke();
    }

    private void OnReachMaxDistanceHandler() {
        _currentProjectile.ReturnBack();
    }

    public void FishLostHandler() {
        _currentProjectile.ReturnBack();
    }

    public void FishCaughtHandler() {
        _currentProjectile.ReturnBack();
    }

    private void OnDisable() {
        _playerController.OnAimUpdate -= OnAimUpdateHandler;
    }


}
public struct AimData {
    public float ClampedAngle;
    public Vector3 ClampedDirection;
    public bool IsFacingRight;
}

public class TargetHitData {
    public FishController fishController;
    public LookDirection fishDirectionFromPlayer;
}
