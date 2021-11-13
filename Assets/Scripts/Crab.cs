using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour
{
    [SerializeField] private float _walkRange;
    [SerializeField] private bool _faceRight;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private float _pushPower;

    private Vector2 _startPostion;
    private float _lastAtackTime;
    private int _direction = -1;

    private Vector2 _drawPostion
    {
        get
        {
            if (_startPostion == Vector2.zero)
                return transform.position;
            else
                return _startPostion;
        }
    }

    private void Start()
    {
        _startPostion = transform.position;
    }

    private void Update()
    {
        float xPos = transform.position.x;
        if (xPos > _startPostion.x + _walkRange && _faceRight)
        {
            Flip();
        }
        else if (xPos < _startPostion.x - _walkRange && !_faceRight)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = Vector2.right *_direction * _speed;
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_drawPostion, size: new Vector3(x: _walkRange * 2, y: 1, z: 0));
    }

    private void Flip()
    {
        _faceRight = !_faceRight;
        transform.Rotate(0, 180, 0);
        _direction *= -1;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerMover player = other.collider.GetComponent<PlayerMover>();
        if (player != null && Time.time - _lastAtackTime > 0.2f)
        {
            _lastAtackTime = Time.time;
            player.TakeDamage(_damage, _pushPower, transform.position.x);
        } 
    }

}
