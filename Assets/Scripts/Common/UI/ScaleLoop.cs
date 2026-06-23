using UnityEngine;
using DG.Tweening;

public class ScaleLoop : MonoBehaviour
{
    [SerializeField] float _scaleFactor;
    [SerializeField] float _scaleTime;
    private void Awake() {
        transform.DOScale(_scaleFactor, _scaleTime).SetLoops(-1, LoopType.Yoyo);
    }
}
