using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : PausableBehaviour
{
    [Header("Stats")]
    [field: SerializeField] public float Speed { get; set; }
    [field: SerializeField] public float DurationSeconds { get; set; }
    [field: SerializeField] public int Damage { get; set; }

    private Vector2 _velocity;
    private float _ttl;
    internal GameObject bulletOwner;

    //Components
    private Rigidbody2D _rb;
    private Collider2D _collider;

    private void Start()
    {
        //Set
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _ttl = Time.timeSinceLevelLoad + DurationSeconds;
    }

    #region Core

    internal void Configure_bullet(GameObject parent, float bullet_charge)
    {
        //Set
        bulletOwner = parent;

        float max_bullet_size = 2;
        this.gameObject.transform.localScale *= (1 + Mathf.Clamp(bullet_charge, 0, max_bullet_size));
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

    #endregion

    #region Functions

    private List<Collider2D> _overlaping = new();
    private void FixedUpdate()
    {
        _rb.linearVelocity = _velocity;

        var overlaped = Physics2D.OverlapCollider(_collider, _overlaping);
        for (var i = 0; i < overlaped; i++)
        {
            if (_overlaping[i].TryGetComponent<Damageable>(out var damageable))
            {
                //Validate
                if(damageable.gameObject == bulletOwner) { return; }

                //Set
                damageable.TakeDamage(Damage);
                Destroy(gameObject);
            }
            else if (_overlaping[i].TryGetComponent<Bullet>(out var other_bullet))
            {
                //Validate
                if (other_bullet.GetComponent<Bullet>().bulletOwner == bulletOwner) { return; }

                //Call
                Destroy(this.gameObject);
            }
        }
    }

    #endregion
}
