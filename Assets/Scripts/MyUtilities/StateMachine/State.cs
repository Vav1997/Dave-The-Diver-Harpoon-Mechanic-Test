using System;
using UnityEngine;

public abstract class State {
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();

    public abstract void OnMyTriggerEnter(Collider other);
}
