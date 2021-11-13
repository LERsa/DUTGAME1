using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _lifetime;

    void Start()
    {
        Invoke(nameof(Destroy), 1f);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMover player = other.GetComponent<PlayerMover>();
        if (player != null)
        {
            player.TakeDamage(_damage);
        }

        Destroy();
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }


}
