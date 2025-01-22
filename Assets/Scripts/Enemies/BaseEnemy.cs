using UnityEngine;

public class BaseEnemy : PausableBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _bulletOrigin;
    [SerializeField] private Bullet _bulletPrefab;

    [Header("Stats")]
    [SerializeField] private float _speed;
    [SerializeField] private int _points;
    [SerializeField] private float _minDelayBetweenShots;
    [SerializeField] private float _maxDelayBetweenShots;


    private Rigidbody2D _rb;
    private Vector2 _velocity;
    private Damageable _damageable;
    private float _timeNextShot;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _damageable = GetComponent<Damageable>();
        _damageable.OnDie.AddListener(OnDestroyed);
        CalculateNextShot();
    }

    #region Core

    private void OnDestroyed()
    {
        Destroy(gameObject);
        _shared.AddScore(_points);
    }

    protected override void DoUpdate()
    {
        _velocity = Vector2.left * _speed;

        if (Time.timeSinceLevelLoad > _timeNextShot)
        {
            CalculateNextShot();
            Shoot();
        }
    }

    protected override void OnPause()
    {
        _velocity = Vector2.zero;
    }

    protected override void OnUnpause()
    {
        _timeNextShot = Time.timeSinceLevelLoad + _timeNextShot;
    }

    #endregion

    private void CalculateNextShot()
    {
        _timeNextShot = Time.timeSinceLevelLoad + Random.Range(_minDelayBetweenShots, _maxDelayBetweenShots);
    }

    private void Shoot()
    {
        var bullet_clone = Instantiate(_bulletPrefab);
        bullet_clone.transform.position = _bulletOrigin.position;
        bullet_clone.transform.right = _bulletOrigin.right;

        //Set
        bullet_clone.GetComponent<Bullet>().Configure_bullet(this.gameObject);
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Despawn"))
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

}
