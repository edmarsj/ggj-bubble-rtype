using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PausableBehaviour
{
    [field: SerializeField] public float Speed { get; set; }
    [field: SerializeField] public float DurationSeconds { get; set; }
    [field: SerializeField] public int Damage { get; set; }

    private Rigidbody2D _rb;
    private Collider2D _collider;
    private Vector2 _velocity;
    private float _ttl;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _ttl = Time.timeSinceLevelLoad + DurationSeconds;
    }

    protected override void DoUpdate()
    {
        _velocity = transform.right * Speed;

        if (Time.timeSinceLevelLoad > _ttl)
        {
            Destroy(gameObject);
        }
    }

    protected override void OnPause()
    {
        _velocity = Vector2.zero;
    }

    protected override void OnUnpause()
    {
        _ttl = Time.timeSinceLevelLoad + _ttl;
    }

    private List<Collider2D> _overlaping = new();
    private void FixedUpdate()
    {
        _rb.linearVelocity = _velocity;

        var overlaped = Physics2D.OverlapCollider(_collider, _overlaping);
        for (var i = 0; i < overlaped; i++)
        {
            if (_overlaping[i].TryGetComponent<Damageable>(out var damageable))
            {
                damageable.TakeDamage(Damage);
                Destroy(gameObject);
            }
        }
    }


}
