using System;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    private Transform _target;
    private bool _isInitialized;
    private bool _isStopped = false;
    public void Initialize(Transform target) {
        _target = target;
        _isInitialized = true;
    }

    public void StopFollow() {
        _isStopped = true;
    }

    private void Update() {
        if(!_isInitialized || _isStopped) {
            return;
        }

        if(_target == null) {
            //Destroy(gameObject);
            return;
        }

        transform.position = _target.position;
        transform.rotation = _target.rotation;
    }
}
