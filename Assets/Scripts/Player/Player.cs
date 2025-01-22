using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : PausableBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _bulletOrigin;
    [Header("Setup")]
    [SerializeField] private float _speed;    
    [SerializeField] private Bullet _bulletPrefab;

    private Rigidbody2D _rb;

    private Vector2 _velocity = Vector2.zero;

    private Damageable _damageable;
    public float Life => _damageable.CurrentLife;

    private void Awake()
    {
        _shared.Player = this;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();       
        _damageable = GetComponent<Damageable>();
        _damageable.OnDie.AddListener(OnPlayerDie);
    }

    private void OnPlayerDie()
    {
        SceneManager.LoadScene("GameOver");
    }

    protected override void DoUpdate()
    {
        var levelModificators = _shared.CurrentLevel.Modificators;

        var vertical = Input.GetAxisRaw("Vertical") * levelModificators.Vertical;
        var horizontal = Input.GetAxisRaw("Horizontal") * levelModificators.Horizontal;

        _velocity = levelModificators.InvertControls ? new Vector2(vertical, horizontal)
                                                      : new Vector2(horizontal, vertical);

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        var clone = Instantiate(_bulletPrefab);
        clone.transform.position = _bulletOrigin.position;
    }

    private void FixedUpdate()
    {
        var levelModificators = _shared.CurrentLevel.Modificators;
        _rb.linearVelocity = _velocity * _speed * levelModificators.SpeedScale;
    }
}
