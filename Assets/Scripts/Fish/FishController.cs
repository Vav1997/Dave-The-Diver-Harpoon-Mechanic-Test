using TreeEditor;
using UnityEngine;

public class FishController : MonoBehaviour
{
    [SerializeField] private float _minMovingDistance;
    [SerializeField] private float _maxMovingDistance;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _arrivalThreshold = 0.1f;

    private State _state;
    private Vector3 _originPosition;
    private Vector3 _currentTarget;

    private void Start() {
        _originPosition = transform.position;
        PickNewDestination();
    }

    private void Update() {
        if(_state == State.Moving) {
            Vector3 directionToTarget = _currentTarget - transform.position;
            //float directionAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, directionAngle), Time.deltaTime);
            directionToTarget.Normalize();
            transform.position += directionToTarget * _moveSpeed * Time.deltaTime;

            if(Vector3.Distance(transform.position, _currentTarget) <= _arrivalThreshold) {
                PickNewDestination();
            }
        }
    }

    private void PickNewDestination() {
        float randomX = Random.Range(_minMovingDistance, _maxMovingDistance) * (Random.value > 0.5f ? 1 : -1);
        float randomY = Random.Range(_minMovingDistance, _maxMovingDistance) * (Random.value > 0.5f ? 1 : -1);

        _currentTarget = _originPosition + new Vector3(randomX, randomY, 0);
    }

    public void SetHarpoonHit(Transform fishAttachTransform) {
        transform.parent = fishAttachTransform;
        transform.localPosition = Vector3.zero;
        transform.rotation = fishAttachTransform.rotation;
        _state = State.OnHarpoon;
    }

    public void ReleaseFromHarpoon() {
        transform.parent = null;
        _state = State.Moving;
        transform.rotation = Quaternion.identity;
    }

    public enum State {
        Moving,
        OnHarpoon,
        Dead
    }
}
