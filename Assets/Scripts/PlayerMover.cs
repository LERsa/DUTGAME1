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
    [SerializeField] private LayerMask _whatIsCell;
    [SerializeField] private Collider2D _headCollider;
    [SerializeField] private float _headChekerRadius;
    [SerializeField] private Transform _headCheker;

    [Header(("Animation"))]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _runAnimatorKey;
    [SerializeField] private string _jumpAnimatorKey;
    [SerializeField] private string _crawlAnimatorKey;

    private float _horizontalDirection;
    private float _verticalDirection;
    private bool _jump;
    private bool _crawl;

    public bool CanClimb {private get; set; }




    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _horizontalDirection = Input.GetAxisRaw("Horizontal");
        _verticalDirection = Input.GetAxisRaw("Vertical");
        _animator.SetFloat(_runAnimatorKey, value:Mathf.Abs(_horizontalDirection));
        
        
       
        if(_horizontalDirection > 0 && _spriterenderer.flipX)
        {
            _spriterenderer.flipX = false;
        }
        else if (_horizontalDirection < 0 && !_spriterenderer.flipX)
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
        bool canStand = !Physics2D.OverlapCircle((Vector2)_headCheker.position, _headChekerRadius, _whatIsCell);

        _animator.SetBool(_jumpAnimatorKey, !canJump);
        _animator.SetBool(_crawlAnimatorKey, !_headCollider.enabled);


        _rigidbody.velocity = new Vector2(x: _horizontalDirection * _speed, _rigidbody.velocity.y);

        if (CanClimb)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x,y: _verticalDirection * _speed);
            _rigidbody.gravityScale = 0;
        }
        else
        {
            _rigidbody.gravityScale = 1;
        }

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

    public void AddMoney(int money)
    {
        Debug.Log(message: "Money raised: " + money);
    }
    
}

