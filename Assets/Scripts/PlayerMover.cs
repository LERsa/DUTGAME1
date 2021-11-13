using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Serialization;



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
    [SerializeField] private string _hurtAnimatorKey;
    [SerializeField] private string _deathAnimatorKey;
    [SerializeField] private string _attackAnimatorKey;
    [SerializeField] private string _castAnimatorKey;
    
 

    [Header(("UI"))]
    [SerializeField] private TMP_Text _amountOfCoinsText;
    [SerializeField] private Slider _HPBar;

    [Header("Attack")]
    [SerializeField] private int _swordDamage;
    [SerializeField] private Transform _swordAttackPoint;
    [SerializeField] private float _swordAttackRadius;
    [SerializeField] private LayerMask _whatIsEnemy;
    [SerializeField] private int _skillDamage;
    [SerializeField] private Transform _skillCastPoint;
    [SerializeField] private float _skillLength;
    [SerializeField] private LineRenderer _castLine;


    [SerializeField] private bool _faceRight;

    private float _lastPushTime;
    private float _horizontalDirection;
    private float _verticalDirection;
    private bool _jump;
    private bool _crawl;
    private int _coinsAmount;
    private bool _needToAttack;
    private int _HP;
    private bool _needToCast;



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



        if (_animator.GetBool(_hurtAnimatorKey))
        {
            return;
        }

        if (_animator.GetBool(_deathAnimatorKey))
        {
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            _needToAttack = true;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            _needToCast = true;
        }


        if (_horizontalDirection > 0 && !_faceRight)
        {
            Flip();
        }
        else if (_horizontalDirection < 0 && _faceRight)
        {
            Flip();
        }


        Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2)_groundCheker.position, _groundchekerRadius);
        
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jump = true;

        }


        _crawl = Input.GetKey(KeyCode.C);
        
    }

    private void Flip()
    {
        _faceRight = !_faceRight;
        transform.Rotate(0, 180, 0);
    }

    private void StartCast()
    {
        if (_animator.GetBool(_castAnimatorKey))
        {
            return;
        }

        _animator.SetBool(_castAnimatorKey, true);
    }

    private void StartAttack()
    {
        if (_animator.GetBool(_attackAnimatorKey))
        {
            return;
        }

        _animator.SetBool(_attackAnimatorKey, true);
    }

    private void Attack()
    {
        Collider2D[] targets = Physics2D.OverlapBoxAll(_swordAttackPoint.position,
            new Vector2(_swordAttackRadius, _swordAttackRadius), _whatIsEnemy);

        foreach (var target in targets)
        {
            Shooter shooter = target.GetComponent<Shooter>();
            if (shooter != null)
            {
                shooter.TakeDamage(_swordDamage);
            }
        }
        _animator.SetBool(_attackAnimatorKey, false);
        _needToAttack = false;
    }

    private void Cast()
    {
        RaycastHit2D[] hits =
            Physics2D.RaycastAll(_skillCastPoint.position, transform.right, _skillLength, _whatIsEnemy);
        foreach (var hit in hits)
        {
            Shooter shooter = hit.collider.GetComponent<Shooter>();
            if (shooter != null)
            {
                shooter.TakeDamage(_skillDamage);
            }
        }
        _animator.SetBool(_castAnimatorKey, false);
        _needToCast = false;
        _castLine.SetPosition(0, _skillCastPoint.position);
        _castLine.SetPosition(1, _skillCastPoint.position + transform.right * _skillLength);
        _castLine.enabled = true;
        Invoke(nameof(DisableLine), 0.1f);
    }

    private void DisableLine()
    {
        _castLine.enabled = false;
    }


    private void FixedUpdate()
    {
        bool canJump = Physics2D.OverlapCircle((Vector2)_groundCheker.position, _groundchekerRadius, _whatIsGround);
        bool canStand = !Physics2D.OverlapCircle((Vector2)_headCheker.position, _headChekerRadius, _whatIsCell);

        _animator.SetBool(_jumpAnimatorKey, !canJump);
        _animator.SetBool(_crawlAnimatorKey, !_headCollider.enabled);

        if (_animator.GetBool(_hurtAnimatorKey))
        {
            if (canJump && Time.time - _lastPushTime > 0.2f)
            {

                _animator.SetBool(_hurtAnimatorKey, false);
            }

            _needToAttack = false;
            _needToCast = false;
            return;
        }


        if (_animator.GetBool(_hurtAnimatorKey))
        {
            if (canJump && Time.time - _lastPushTime > 0.2f)
            {

                _animator.SetBool(_hurtAnimatorKey, false);
            }

            return;
        }


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

        if (_needToAttack)
        {
            StartAttack();
            _horizontalDirection = 0;
        }

        if (_needToCast)
        {
            StartCast();
            _horizontalDirection = 0;
        }

        if (!_headCollider.enabled)
        {
            _needToAttack = false;
            _needToCast = false;
            return;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundCheker.position, _groundchekerRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_headCheker.position, _headChekerRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_swordAttackPoint.position, new Vector3(_swordAttackRadius, _swordAttackRadius, 0));

    }

    public void AddMoney(int money)
    {
        Coins += money;
    }

    public void TakeDamage(int damage, float pushPower = 0, float posX = 0)
    {
        if (_animator.GetBool(_hurtAnimatorKey))
        {
            return;
        }

        CurrentHP -= damage;
        Debug.Log(message: "HP LEft: " + CurrentHP);
        if(CurrentHP <= 0)
        {
            _animator.SetBool(_deathAnimatorKey, true);
            Invoke(nameof(ReloadScene), time: 3f);
        }

        if (pushPower != 0 && Time.time - _lastPushTime > 0.5f)
        {
            _lastPushTime = Time.time;
            int direction = posX > transform.position.x ? -1 : 1;
            _rigidbody.AddForce(new Vector2(direction * pushPower, pushPower));
            _animator.SetBool(_hurtAnimatorKey, true);
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

