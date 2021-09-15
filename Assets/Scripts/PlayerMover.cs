using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    [SerializeField]private float _speed;
    [SerializeField] private SpriteRenderer _spriterenderer;
    [SerializeField] private float _jumpforce;
    [SerializeField] private Transform _groundCheker;
    [SerializeField] private float _groundchekerRadius;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Collider2D _headCollider;
    [SerializeField] private float _headChekerRadius;
    [SerializeField] private Transform _headCheker;


    private float _direction;
    private bool _jump;
    private bool _crawl;



    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _direction = Input.GetAxisRaw("Horizontal");
        
        
       
        if(_direction > 0 && _spriterenderer.flipX)
        {
            _spriterenderer.flipX = false;
        }
        else if (_direction < 0 && !_spriterenderer.flipX)
        {
            _spriterenderer.flipX = true;
        }
         
       
        Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2)_groundCheker.position, _groundchekerRadius);
        
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jump = true;

        }


        _crawl = Input.GetKey(KeyCode.C);
        
    }

    private void FixedUpdate()
    {
        bool canJump = Physics2D.OverlapCircle((Vector2)_groundCheker.position, _groundchekerRadius, _whatIsGround);
        bool canStand = !Physics2D.OverlapCircle((Vector2)_headCheker.position, _headChekerRadius, _whatIsGround);


        _rigidbody.velocity = new Vector2(x: _direction * _speed, _rigidbody.velocity.y);

        _headCollider.enabled = !_crawl && canStand;
       
        if (_jump && canJump == true)
        {
            _rigidbody.AddForce(Vector2.up * _jumpforce);
            _jump = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundCheker.position, _groundchekerRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_headCheker.position, _headChekerRadius);

    }
}
