public abstract class HarpoonStateMachineState : State {
    public HarpoonProjectileController _controller;
    public HarpoonStateMachineState(HarpoonProjectileController controller) {
        _controller = controller;
    }
}