using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class StateMachine
{
    private StateNode _currentStateNode;
    private Dictionary<Type, StateNode> _stateNodes = new();
    
    private List<StateTransition> _transitions = new();

    public void SetState(State newState) {
        _currentStateNode = _stateNodes[newState.GetType()];
        _currentStateNode.State.EnterState();
    }

    public void ChangeState(State newState) {
        if(newState == _currentStateNode.State) {
            return;
        }

        State previousState = _currentStateNode.State;
        State nextState = _stateNodes[newState.GetType()].State;

        previousState?.ExitState();
        nextState?.EnterState();

        _currentStateNode = _stateNodes[newState.GetType()];

    }

    public void Update() {
        var transition = GetTransition();
        if(transition != null) {
            ChangeState(transition.To);
            return;
        }

        _currentStateNode.State?.UpdateState();
    }

    public void OnMyTriggerEnter(Collider other) {
        _currentStateNode.State?.OnMyTriggerEnter(other);
    }

    private StateTransition GetTransition() {
        foreach(var transition in _transitions) {
            if(transition.Condition.Evaluate()) {
                return transition;
            }
        }

        foreach(var currentStateTransition in _currentStateNode.Transitions) {
            if(currentStateTransition.Condition.Evaluate()) {
                return currentStateTransition;
            }
        }

        return null;
    }

    public void AddTransition(State from, State to, FuncPredicate condition) {
        GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
    }

    public void AddAnyTransition(State to, FuncPredicate condition) {
        _transitions.Add(new StateTransition(to, condition));
    }

    public StateNode GetOrAddNode(State state) {
        var stateNode = _stateNodes.GetValueOrDefault(state.GetType());
        if(stateNode != null) {
            return stateNode;
        }

        stateNode = new StateNode(state);
        _stateNodes.Add(state.GetType(), stateNode);
        return stateNode;
    }
}
