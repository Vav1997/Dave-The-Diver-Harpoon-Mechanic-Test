using UnityEngine;

public class StayBetweenTwoPositions : MonoBehaviour
{
    private Transform _transformOne;
    private Transform _transformTwo;
    private bool _isInitialized = false;

    public void Initialize(Transform transformOne, Transform transformTwo) {
        _transformOne = transformOne;
        _transformTwo = transformTwo;
        _isInitialized = true;
    }

    private void Update() {
        if(!_isInitialized) {
            return;
        }

        if(_transformOne == null || _transformTwo == null) {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.Lerp(_transformOne.position, _transformTwo.position, 0.5f);
    }
}

