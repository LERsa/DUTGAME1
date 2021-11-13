using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Shooter : MonoBehaviour
{
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _whatIsPlayer;
    [SerializeField] private Transform _muzzle;
    [SerializeField] Rigidbody2D _bullet;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private bool _faceRight;
    [SerializeField] private Slider _hpBar;
    [SerializeField] private int _maxHp;
    [SerializeField] private GameObject _enemySystem;

    [Header(("Animation"))]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _shootAnimationKey;

    private bool _canShoot;
    private int _currentHp;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(_attackRange * 2, 1, 0));
    }

    private void Start()
    {
        _hpBar.maxValue = _maxHp;
        ChangeHp(_maxHp);
    }

    public void ChangeHp(int hp)
    {
        _currentHp = hp;
        if (_currentHp <= 0)
        {
            Destroy(_enemySystem);
        }
        _hpBar.value = hp;
    }

    public void TakeDamage(int damage)
    {
        _currentHp -= damage;
        _hpBar.value = _currentHp;
        if (_currentHp <= 0)
        {
            Destroy(_enemySystem);
        }

    }

    private void FixedUpdate()
    {
        if (_canShoot)
        {
            return;
        }
        CheckIfCanShoot();
    }

    private void CheckIfCanShoot()
    {
        Collider2D player = Physics2D.OverlapBox(transform.position, new Vector2(_attackRange * 2, 1), 0, _whatIsPlayer);
        if (player != null)
        {
            StartShoot(player.transform.position);
            _canShoot = true;
        }
        else
        {
            _canShoot = false;
        }
    }



    public void Shoot()
    {
        Rigidbody2D bullet = Instantiate(_bullet, _muzzle.position, Quaternion.identity);
        bullet.velocity = _projectileSpeed * transform.right;
        _animator.SetBool(_shootAnimationKey, false);
        Invoke(nameof(CheckIfCanShoot), 1f);
    }

    private void StartShoot(Vector2 playerPosition)
    {
        float posX = transform.position.x;
        if (posX < playerPosition.x && !_faceRight || posX > playerPosition.x && _faceRight)
        {
            transform.Rotate(0, 180, 0);
            _faceRight = !_faceRight;
        }
        _animator.SetBool(_shootAnimationKey, true);
    }

}
