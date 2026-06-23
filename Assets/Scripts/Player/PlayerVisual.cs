using System;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _playerSpriteRenderer;

    private PlayerController _controller;


    public void Initialize(PlayerController controller) {
        _controller = controller;
        _controller.OnPlayerLookDirectionChanged += OnPlayerLookDirectionChangedHandler;
    }

    private void OnPlayerLookDirectionChangedHandler(LookDirection lookDirection) {
        _playerSpriteRenderer.flipX = lookDirection == LookDirection.Left;
    }

    private void OnDisable() {
        _controller.OnPlayerLookDirectionChanged -= OnPlayerLookDirectionChangedHandler;
    }
}
