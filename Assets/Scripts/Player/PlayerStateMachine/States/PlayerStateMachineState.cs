
public abstract class PlayerStateMachineState : State {
    protected PlayerController _controller;
    public PlayerStateMachineState(PlayerController controller) {
        _controller = controller;
    }
}