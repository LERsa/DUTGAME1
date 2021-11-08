using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Lever : MonoBehaviour
{
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private Chest _chest;
    private Sprite _inactiveSprite;

    private SpriteRenderer _spriteRenderer;
    private bool _activated;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _inactiveSprite = _spriteRenderer.sprite;
    }

  
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMover player = other.GetComponent<PlayerMover>();
        if (player != null && !_activated)
        {
            _spriteRenderer.sprite = _activeSprite;
            _chest.Activated = true;                            
            _chest._spriteRenderer.sprite = _chest._activeSprite; // изменение спрайта сундука в скрипте "chest"
            _activated = true;
        }

    }
}
