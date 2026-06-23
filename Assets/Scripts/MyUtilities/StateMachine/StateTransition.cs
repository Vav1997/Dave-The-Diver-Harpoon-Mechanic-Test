using System;

public class StateTransition {
    public State To { get; private set; }
    public FuncPredicate Condition { get; private set; }

    public StateTransition(State to, FuncPredicate condition) {
        To= to;
        Condition = condition;
    }
}

