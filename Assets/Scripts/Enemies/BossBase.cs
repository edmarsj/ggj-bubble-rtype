using UnityEngine;

namespace Game.Enemies
{
    public class BossBase : PausableBehaviour
    {
        [field: Header("Stats")]
        [field: SerializeField] public string DisplayName { get; set; }

        [Space(10)]
        [SerializeField] private Transform _bulletOrigin;
        [SerializeField] private Bullet[] _bulletPrefabs;
        [SerializeField] private float _speed;
        [SerializeField] private int _points;
        [Range(0f, 1f)]
        [SerializeField] private float _minDelayBetweenShots;
        [Range(0f, 5f)]
        [SerializeField] private float _maxDelayBetweenShots;
        [SerializeField] private float _bulletCharge_value;

        private Rigidbody2D _rb;
        private Vector2 _velocity;
        private Damageable _damageable;
        private float _timeNextShot;

        public float LifePercent => _damageable.LifePercent;

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
            _rb = GetComponent<Rigidbody2D>();
            _damageable = GetComponent<Damageable>();
            _damageable.OnDie.AddListener(OnDestroyed);
        }

        #region Core

        private void OnDestroyed()
        {
            Destroy(gameObject);
            _shared.AddScore(_points);
            _shared.BossDefeated = true;

            _shared.OnEndBossBattle.Invoke();

            //Drop worm hole
            var worm_hole = Instantiate(Resources.Load("Prefabs/Map/Worm_hole"), transform.position, Quaternion.identity);
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

        #endregion

        private void FixedUpdate()
        {
            _rb.linearVelocity = _velocity;
        }

        #region Main functions

        private void CalculateNextShot()
        {
            _timeNextShot = Time.timeSinceLevelLoad + Random.Range(_minDelayBetweenShots, _maxDelayBetweenShots);
        }

        private void Shoot()
        {
            var bullet_clone = Instantiate(_bulletPrefabs[Random.Range(0, _bulletPrefabs.Length)]);
            bullet_clone.transform.position = _bulletOrigin.position;
            bullet_clone.transform.right = _bulletOrigin.right;

            //Set
            bullet_clone.GetComponent<Bullet>().Configure_bullet(this.gameObject, _bulletCharge_value);
        }

        #endregion
    }
}
