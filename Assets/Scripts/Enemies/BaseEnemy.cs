using Game.Particles;
using UnityEngine;

namespace Game.Enemies
{
    public class BaseEnemy : PausableBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _bulletOrigin;
        [SerializeField] private Bullet _bulletPrefab;
        private Enemy_scriptable _My_scriptable;

        [Header("Stats")]
        [SerializeField] private float _speed;
        [SerializeField] private int _points;
        [SerializeField] private float _minDelayBetweenShots;
        [SerializeField] private float _maxDelayBetweenShots;
        [Space(10)]
        [SerializeField] private bool _canDrop_powerUps;
        [Range(0f, 1f)]
        [SerializeField] private float _powerUp_drop_chance;

        private Rigidbody2D _rb;
        private SpriteRenderer Render;
        private Vector2 _velocity;
        private Damageable _damageable;
        private float _timeNextShot;

        #region Triggers
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

        #endregion

        private void Awake()
        {
            //Set
            _rb = GetComponent<Rigidbody2D>();
            Render = this.GetComponentInChildren<SpriteRenderer>();
            _damageable = GetComponent<Damageable>();
            _damageable.OnDie.AddListener(OnDestroyed);
            CalculateNextShot();
        }

        #region Core

        private void OnDestroyed()
        {
            //Set
            _shared.AddScore(_points);

            //Call
            Try_drop_powerUp();

            //Particles
            Particle_system.Create_particle("Debris_particle", transform.position);

            Destroy(gameObject);
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

        #region Main functions

        internal void Configure_enemy(Enemy_scriptable scriptable)
        {
            //Set
            _My_scriptable = scriptable;

            _speed = scriptable.Speed;
            _points = scriptable.Points;
            _minDelayBetweenShots = scriptable.minDelayBetweenShots;
            _maxDelayBetweenShots = scriptable.maxDelayBetweenShots;

            Render.sprite = scriptable.Sprite;

            this.gameObject.transform.localScale *= scriptable.Scale_multiplier;
        }

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
            bullet_clone.GetComponent<Bullet>().Configure_bullet(this.gameObject, 0);
        }

        private void Try_drop_powerUp()
        {
            float c = Random.Range(0f, 1f);

            if (c <= _powerUp_drop_chance) 
            { 
                var power_up_clone = Instantiate(Resources.Load("Prefabs/Map/Power_up"), transform.position, Quaternion.identity);
            }
        }

        #endregion

        private void FixedUpdate()
        {
            _rb.linearVelocity = _velocity;
        }
    }
}