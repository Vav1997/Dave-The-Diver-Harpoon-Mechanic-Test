using System.Collections.Generic;

public class StateNode {
    public State State { get; }
    public List<StateTransition> Transitions { get; }

    public StateNode(State state) {
        State = state;
        Transitions = new List<StateTransition>();
    }

    public void AddTransition(State to, FuncPredicate condition) {
        Transitions.Add(new StateTransition(to, condition));
    }
}