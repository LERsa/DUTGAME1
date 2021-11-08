using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    [SerializeField] private int _maxHP;
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

    [Header(("UI"))]
    [SerializeField] private TMP_Text _amountOfCoinsText;
    [SerializeField] private Slider _HPBar;

    private float _horizontalDirection;
    private float _verticalDirection;
    private bool _jump;
    private bool _crawl;
    private int _coinsAmount;
    private int _HP;
    public int Coins 
    {
        get => _coinsAmount;
        
        set
        {
            _coinsAmount = value;
            _amountOfCoinsText.text = value.ToString();
        }
    }

    private int CurrentHP
    {
        get => _HP;

        set
        {
            _HP = value;
            _HPBar.value = value;
        }
    }




    public bool CanClimb {private get; set; }




    void Start()
    {
        Coins = 0;
        _rigidbody = GetComponent<Rigidbody2D>();
        _HPBar.maxValue = _maxHP;
        CurrentHP = _maxHP;
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
        Coins += money;
    }

    public void TakeDamage(int damage)
    {
        CurrentHP -= damage;
        Debug.Log(message: "HP LEft: " + CurrentHP);
        if(CurrentHP <= 0)
        {
            gameObject.SetActive(false);
            Invoke(nameof(ReloadScene), time: 1f);
        }
      
    }

    public void HealHP(int hpGained)
    {
        int missingHP = _maxHP - CurrentHP;
        int pointsToAdd = missingHP > hpGained ? hpGained : missingHP;
        CurrentHP += pointsToAdd;
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}

