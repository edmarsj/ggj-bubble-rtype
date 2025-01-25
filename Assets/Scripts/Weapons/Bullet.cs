using Game.Pools;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Projectiles
{
    [RequireComponent(typeof(PoolableObject))]
    public class Bullet : PausableBehaviour
    {
        public enum Bullet_behaviours { Normal, Bi_divide, Tri_divide, Spike }

        [Header("Stats")]
        [field: SerializeField] public Bullet_behaviours Bullet_behaviour;
        [field: SerializeField] public float Speed { get; set; }
        [field: SerializeField] public float DurationSeconds { get; set; }
        [Range(0f, 2f)]
        [field: SerializeField] private float TimeToActive { get; set; }
        [field: SerializeField] public int Damage { get; set; }

        [SerializeField] private float _lifeTime;
        private Vector2 _velocity;
        private float _ttl;
        internal GameObject bulletOwner;
        private bool _dead = false;
        private Vector3 _initialLocalScale;

        //Components
        private Rigidbody2D _rb;
        private Collider2D _collider;
        private PoolableObject _poolableObject;
        private float _damageMultiplier = 1f;

        private void Awake()
        {
            _initialLocalScale = transform.localScale;
        }

        private void Start()
        {
            //Set
            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _poolableObject = GetComponent<PoolableObject>();

        }

        private void OnEnable()
        {
            _ttl = Time.timeSinceLevelLoad + DurationSeconds;
            _dead = false;
        }

        #region Core

        internal void Configure_bullet(GameObject parent, float bullet_charge)
        {
            //Set
            bulletOwner = parent;
            _damageMultiplier = 1f + bullet_charge;

            float max_bullet_size = 2;
            this.gameObject.transform.localScale = _initialLocalScale * Mathf.Clamp(bullet_charge, 1f, max_bullet_size);
        }

        internal void Configure_bullet(GameObject parent, float bullet_charge, Bullet_behaviours behaviour)
        {
            //Set
            bulletOwner = parent;
            Bullet_behaviour = behaviour;

            float max_bullet_size = 2;
            this.gameObject.transform.localScale = _initialLocalScale * Mathf.Clamp(bullet_charge, 1f, max_bullet_size);
        }

        internal void Rotate_bullet(float angle)
        {
            //Set
            this.gameObject.transform.localEulerAngles = new Vector3(this.gameObject.transform.localEulerAngles.z, this.gameObject.transform.localEulerAngles.y, angle);
        }

        protected override void DoUpdate()
        {
            //Set
            _velocity = transform.right * Speed;
            _lifeTime += Time.deltaTime;

            if (Time.timeSinceLevelLoad > _ttl)
            {
                Destroy(gameObject);
            }

            //Call
            Control_bullet_behaviour();
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

        private List<Collider2D> _overlaping = new();
        private void FixedUpdate()
        {
            if (_dead)
            {
                return;
            }

            _rb.linearVelocity = _velocity;

            var overlaped = Physics2D.OverlapCollider(_collider, _overlaping);
            for (var i = 0; i < overlaped; i++)
            {
                if (_overlaping[i].TryGetComponent<Damageable>(out var damageable))
                {
                    //Validate
                    if (damageable.gameObject == bulletOwner) { return; }

                    //Set
                    damageable.TakeDamage(Damage * _damageMultiplier);
                    _dead = true;
                    _poolableObject.ReturnToCache();
                }
                else if (_overlaping[i].TryGetComponent<Bullet>(out var other_bullet))
                {
                    //Validate
                    if (other_bullet.GetComponent<Bullet>().bulletOwner == bulletOwner) { return; }

                    //Set
                    _dead = true;
                    //Call
                    _poolableObject.ReturnToCache();
                }
                else if (_overlaping[i].CompareTag("Obstacle"))
                {
                    //Set
                    _dead = true;
                    //Call
                    _poolableObject.ReturnToCache();
                }
            }
        }

        #region Main functions

        private void Control_bullet_behaviour()
        {
            if (_lifeTime >= TimeToActive)
            {
                switch (Bullet_behaviour)
                {
                    case Bullet_behaviours.Normal:
                        break;

                    case Bullet_behaviours.Bi_divide:
                        Divide_bullet(2);
                        break;

                    case Bullet_behaviours.Tri_divide:
                        Divide_bullet(3);
                        break;

                    case Bullet_behaviours.Spike:
                        Divide_bullet(4);
                        break;
                }
            }
        }

        private void Divide_bullet(int ammount)
        {
            List<Bullet> bullets_list = new List<Bullet>();

            for (int i = 0; i < ammount; i++)
            {
                var bullet_clone = PoolingSystem.Instance.Get<Bullet>(_poolableObject.CacheKey);
                bullet_clone.transform.position = transform.position;
                bullet_clone.transform.eulerAngles = transform.eulerAngles;
                bullet_clone.transform.localScale = transform.localScale;
                bullet_clone.Configure_bullet(bulletOwner, 0, Bullet_behaviours.Normal);

                bullets_list.Add(bullet_clone);
            }

            switch (ammount)
            {
                case 2:
                    bullets_list[0].Rotate_bullet(15);
                    bullets_list[1].Rotate_bullet(-15);
                    break;

                case 3:
                    bullets_list[0].Rotate_bullet(15);
                    bullets_list[1].Rotate_bullet(0);
                    bullets_list[2].Rotate_bullet(-15);
                    break;

                case 4:
                    bullets_list[0].Rotate_bullet(0);
                    bullets_list[1].Rotate_bullet(90);
                    bullets_list[2].Rotate_bullet(-180);
                    bullets_list[3].Rotate_bullet(-90);
                    break;
            }

            _poolableObject.ReturnToCache();
        }

        #endregion
    }

}