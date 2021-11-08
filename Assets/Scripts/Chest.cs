using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private int _coinsamount;
    [SerializeField] public Sprite _activeSprite;
    private Sprite _inactiveSprite;                       // ��������� ������� �������, ��������� � ������� "lever"
    public SpriteRenderer _spriteRenderer { get; set; }               // ���������� �������� ������� public, ����� �� ���������� �������� ����������

    public bool Activated;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _inactiveSprite = _spriteRenderer.sprite;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!Activated)
            return;
        PlayerMover player = other.GetComponent<PlayerMover>();
        if(player != null)
        {
            player.AddMoney(player.Coins); 
            Destroy(gameObject); // ������ ��������� ����� 1-�� �������������, ����� ���� ����, ������� ��������� ���������� ������� ������

        }
    }

}
