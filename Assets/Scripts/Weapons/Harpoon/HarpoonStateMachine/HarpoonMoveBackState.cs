using UnityEngine;

public class HarpoonMoveBackState : HarpoonStateMachineState {

    private ProjectileData _projectileData;
    private float _previousDistanceToOrigin = float.MaxValue;
    public bool ReturnedBack { get; private set; }
    public HarpoonMoveBackState(HarpoonProjectileController controller, ProjectileData data) : base(controller) {
        _projectileData = data;
    }

    public override void EnterState() {
        
    }
    public override void UpdateState() {
        var moveBackDirection = (_projectileData.projectileSpawnTransform.position - _controller.transform.position).normalized;
        _controller.transform.position += moveBackDirection * _projectileData.moveSpeed * Time.deltaTime;

        float currentDistance = Vector3.Distance(_controller.transform.position, _projectileData.projectileSpawnTransform.position);

        if(currentDistance < 0.2f || currentDistance > _previousDistanceToOrigin) {
            ReturnedBack = true;
            _controller.harpoonReturnedBackHandler();
        }

        _previousDistanceToOrigin = currentDistance;
    }

    public override void ExitState() {
        
    }

    public override void OnMyTriggerEnter(Collider other) {

    }
}