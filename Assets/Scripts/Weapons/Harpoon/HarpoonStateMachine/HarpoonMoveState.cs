using Unity.Mathematics;
using UnityEngine;
using static HarpoonProjectileController;

public class HarpoonMoveState : HarpoonStateMachineState {

    private ProjectileData _projectileData;
    private LayerMask _obsticleLayers;
    public bool ReachedMaxDistance { get; private set; }
    public bool HitObsticle { get; private set; }
    public bool HitTarget { get; private set; }
    public HarpoonMoveState(HarpoonProjectileController controller, ProjectileData projectileData, LayerMask obstacleLayers) : base(controller) {
        _projectileData = projectileData;
        _obsticleLayers = obstacleLayers;
    }

    public override void EnterState() {
        
    }
    public override void UpdateState() {
        float angle = math.atan2(_projectileData.moveDirection.y, _projectileData.moveDirection.x) * Mathf.Rad2Deg;
        _controller.transform.rotation = Quaternion.Euler(0, 0, angle);

        float movedDistance = Vector3.Distance(_projectileData.projectileSpawnTransform.position, _controller.transform.position);
        float movedDistanceInPercentage = movedDistance / _projectileData.flyingDistance;
        if(movedDistance < _projectileData.flyingDistance) {
            float correctedMoveSpeed = _projectileData.moveSpeed;
            if(movedDistanceInPercentage >= 0.65f) {
                var invertedLerp = Mathf.InverseLerp(0.65f, 1, movedDistanceInPercentage);
                correctedMoveSpeed = Mathf.Lerp(_projectileData.moveSpeed, 0.2f, invertedLerp);
            }

            _controller.transform.position += _projectileData.moveDirection * correctedMoveSpeed * Time.deltaTime;
        }
        else {
            ReachedMaxDistance = true;
            _controller.MaxDistanceReachHandler();
        }
    }

    public override void ExitState() {
        
    }

    public override void OnMyTriggerEnter(Collider other) {
        if(((1 << other.gameObject.layer) & _obsticleLayers) != 0) {
            HitObsticle = true;
            return;
        }

        if(other.TryGetComponent(out FishController fishController) && !HitTarget) {
            _controller.FishHitHandler(fishController);
            HitTarget = true;
        }
    }
}
