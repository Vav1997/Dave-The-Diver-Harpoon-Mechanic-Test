using UnityEngine;

public class HarpoonCountdownToReturnState : HarpoonStateMachineState {

    private float _returnBackDelayTimer;
    public bool TimerEnded {  get; private set; }
    public HarpoonCountdownToReturnState(HarpoonProjectileController controller) : base(controller) {
    }

    public override void EnterState() {
        _returnBackDelayTimer = _controller.ReturnBackDelayTime;
    }
    public override void UpdateState() {
		_returnBackDelayTimer -= Time.deltaTime;
        if(_returnBackDelayTimer <= 0) {
            TimerEnded = true;
        }
	}

    public override void ExitState() {
        
    }

    public override void OnMyTriggerEnter(Collider other) {
        
    }
}