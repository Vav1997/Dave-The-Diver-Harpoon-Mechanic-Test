using System;
using UnityEngine;

public class PlayerWalkState : PlayerStateMachineState {

    private float _moveSpeed;
    private LookDirection _previousLookDirection;
    
    public PlayerWalkState(PlayerController controller, float moveSpeed) : base(controller) {
        _moveSpeed = moveSpeed;
    }

    public override void EnterState() {
        
    }

    public override void UpdateState() {
        var movementHorizontal = Input.GetAxisRaw("Horizontal");
        var movementVertical = Input.GetAxisRaw("Vertical");
        Vector3 movementDirection = new Vector3(movementHorizontal, movementVertical, 0).normalized;

        _controller.transform.position += movementDirection * _moveSpeed * Time.deltaTime;

        LookDirection lookDirection;

        if(movementHorizontal > 0) {
            lookDirection = LookDirection.Right;
            _previousLookDirection = lookDirection;
        }
        else if(movementHorizontal < 0) {
            lookDirection = LookDirection.Left;
            _previousLookDirection = lookDirection;
        }
        else {
            lookDirection = _previousLookDirection;
        }

        _controller.LookDirectionChangeHandler(lookDirection);
    }

    public override void ExitState() {
        
    }

    public override void OnMyTriggerEnter(Collider other) {
        
    }
}
