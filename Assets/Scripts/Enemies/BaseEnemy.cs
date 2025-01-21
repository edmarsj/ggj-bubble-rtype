using System;
using UnityEngine;

public class BaseEnemy : PausableBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _points;
    private Rigidbody2D _rb;
    private Vector2 _velocity;
    private Damageable _damageable;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _damageable = GetComponent<Damageable>();
        _damageable.OnDie.AddListener(OnDestroyed);
    }

    private void OnDestroyed()
    {
        Destroy(gameObject);
        _shared.AddScore(_points);
    }

    protected override void DoUpdate()
    {
        _velocity = Vector2.left * _speed;
    }

    protected override void OnPause()
    {
        _velocity = Vector2.zero;
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