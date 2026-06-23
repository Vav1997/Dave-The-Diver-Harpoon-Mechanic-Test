using System;
using UnityEngine;

public class HarpoonProjectileController : MonoBehaviour
{
    [SerializeField] private Transform _ropeAttachTransform;
    [SerializeField] private Transform _fishAttachTransform;
    [SerializeField] private float _returnBackDelayTime;
    [SerializeField] private LayerMask _obsticleLayers;
    [SerializeField] private ParticleSystem _fishCatchParticle;
    [SerializeField] private ParticleSystem _trailParticle;

    private ProjectileData _data;
    
    public float ReturnBackDelayTime => _returnBackDelayTime;
    public Action OnReachMaxDistance;
    public Action<TargetHitData> OnHitTarget;
    public Action OnReturnedBack;

    private StateMachine _stateMachine;
    private HarpoonMoveBackState _moveBackState;

    public void Initialize(ProjectileData data) {
        _data = data;

        //Initialize State machine data
        _stateMachine = new();

        HarpoonMoveState harpoonMoveState = new (this, data, _obsticleLayers);
        HarpoonCountdownToReturnState countdownToReturnState = new(this);
        _moveBackState = new(this, data);
        

        At(harpoonMoveState, countdownToReturnState, new FuncPredicate(() => harpoonMoveState.ReachedMaxDistance && !harpoonMoveState.HitTarget));
        At(harpoonMoveState, _moveBackState, new FuncPredicate(() => harpoonMoveState.HitObsticle));
        At(countdownToReturnState, _moveBackState, new FuncPredicate(() => countdownToReturnState.TimerEnded));

        _stateMachine.SetState(harpoonMoveState);
    }

    public Transform GetRopeAttachTransform() {
        return _ropeAttachTransform;
    }

    private void Update() {
        _stateMachine.Update();
    }

    private void At(State from, State to, FuncPredicate condition) {
        _stateMachine.AddTransition(from, to, condition);
    }

    private void Any(State to, FuncPredicate condition) {
        _stateMachine.AddAnyTransition(to, condition);
    }

    public void ReturnBack() {
        _stateMachine.SetState(_moveBackState);
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider other) {
        _stateMachine.OnMyTriggerEnter(other);
    }

    public void MaxDistanceReachHandler() {
        Debug.Log("should stop the parrticle");
        _trailParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
    public void FishHitHandler(FishController fishController) {
        Debug.Log("fish hit handler is called");
        fishController.SetHarpoonHit(_fishAttachTransform);
        _fishCatchParticle.Play();
        OnHitTarget?.Invoke(new TargetHitData() {
            fishController = fishController,
            fishDirectionFromPlayer = _data.moveDirection.x > 0 ? LookDirection.Right : LookDirection.Left
        });
    }

    public void harpoonReturnedBackHandler() {
        OnReturnedBack?.Invoke();
    }
}


public struct ProjectileData {
    public float moveSpeed;
    public float flyingDistance;
    public Vector3 moveDirection;
    public Transform projectileSpawnTransform;
}