using UnityEngine;
using Game.Projectiles;
using System.Collections;
using Game.Super_powers;
using StarTravellers.Utils;

namespace Game.Enemies
{
    public class BossBase : PausableBehaviour
    {
        [field: Header("Stats")]
        [field: SerializeField] public string DisplayName { get; set; }
        
        [SerializeField] private float _speed;
        [SerializeField] private int _points;
        [Range(0f, 1f)]
        [SerializeField] private float _minDelayBetweenShots;
        [Range(0f, 5f)]
        [SerializeField] private float _maxDelayBetweenShots;
        [SerializeField] private float _bulletCharge_value;

        [Space(10)]
        [Range(0f, 3f)]
        [SerializeField] private float _minDelayBetweenPowers;
        [Range(0f, 10f)]
        [SerializeField] private float _maxDelayBetweenPowers;

        [Space(10)]
        [Header("References")]
        [SerializeField] private Transform _bulletOrigin;
        [SerializeField] private Bullet[] _bulletPrefabs;
        [SerializeField] private SpriteRenderer _mainRenderer;

        private Rigidbody2D _rb;
        private Vector2 _velocity;
        private Damageable _damageable;
        private Super_powers_base _superpowers;
        

        private float _timeNextShot;
        private bool _start_movements;
        private Material _mainMaterial;
        public float LifePercent => _damageable.LifePercent;

        #region Triggers

        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (collision.tag)
            {
                case "Despawn":
                    {
                        Destroy(gameObject);
                        break;
                    }
                case "BossReadyPoint":
                    {
                        //Set
                        Destroy(collision.gameObject);
                        _shared.LevelStage = GameplayStage.BossFight;
                        _shared.OnBeginBossBattle?.Invoke();

                        _start_movements = true;
                        StartCoroutine(Start_patern_movements());
                        StartCoroutine(Randomize_behaviour_paterns());
                        break;
                    }
                default:
                    break;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            //Call
            Destroy(gameObject);
        }

        #endregion

        private void Awake()
        {
            //Set
            _rb = GetComponent<Rigidbody2D>();
            _superpowers = this.gameObject.GetComponent<Super_powers_base>();
            _damageable = GetComponent<Damageable>();
            _damageable.OnDie.AddListener(OnDestroyed);
            _damageable.OnTakeDamage.AddListener(OnTakeDamage);
            _mainMaterial = _mainRenderer.material;
        }

        #region Core

        private void OnTakeDamage()
        {            
            StartCoroutine(AnimationHelpers.AnimationSequence(new System.Func<IEnumerator>[]
            {
                () => AnimationHelpers.SmoothLerp(f => _mainMaterial.SetFloat("_MaskStage",f),0,1,.05f),
                () => AnimationHelpers.SmoothLerp(f => _mainMaterial.SetFloat("_MaskStage",f),1,0,.1f),
            }));
        }

        private void OnDestroyed()
        {
            Destroy(gameObject);
            _shared.AddScore(_points);
            _shared.BossDefeated = true;

            _shared.OnEndBossBattle.Invoke();           
        }

        protected override void DoUpdate()
        {
            //Set
            _velocity = Vector2.left * _speed;

            if (Time.timeSinceLevelLoad > _timeNextShot && _shared.LevelStage == GameplayStage.BossFight)
            {
                CalculateNextShot();
                Shoot();
            }
        }

        protected override void OnPause()
        {
            //Set
            _velocity = Vector2.zero;
        }

        #endregion

        private void FixedUpdate()
        {
            //Move
            if(!_start_movements) { _rb.linearVelocity = _velocity; }
        }

        #region Main functions

        private IEnumerator Start_patern_movements()
        {
            //Set
            float _newSpeed = _speed * 3;
            Vector3 pos_to_go = transform.position;

            while(_start_movements)
            {
                //Get new position
                if(Vector2.Distance(transform.position, pos_to_go) <= 0.3f)
                {
                    _rb.linearVelocity = Vector3.zero;

                    //Set
                    pos_to_go = new Vector3(pos_to_go.x, Random.Range(-5, 5), pos_to_go.z);

                    _newSpeed = Random.Range(_speed * 2.5f, _speed * 4.5f);

                    print(pos_to_go);
                }
                else
                {
                    //Move
                    _rb.linearVelocity = ((pos_to_go - transform.position).normalized) * _newSpeed;
                }

                yield return null;
            }    
        }

        private IEnumerator Randomize_behaviour_paterns()
        {
            while (_start_movements)
            {
                yield return new WaitForSeconds(Random.Range(_minDelayBetweenPowers, _maxDelayBetweenPowers));

                float c = Random.Range(0f, 1f);

                //Reduce shoots interval
                if(c <= 0.15f)
                {
                    //Set
                    _minDelayBetweenShots -= 0.1f;
                    _maxDelayBetweenShots -= 0.1f;

                    _minDelayBetweenShots = Mathf.Clamp(_minDelayBetweenShots, 0, _maxDelayBetweenShots);
                    _maxDelayBetweenShots = Mathf.Clamp(_maxDelayBetweenShots, _minDelayBetweenShots, _maxDelayBetweenShots);
                }
                //Gets faster
                else if (c <= 0.3f)
                {
                    //Set
                    _speed += 0.25f;
                }
                //Use laser_barrage
                else if (c <= 0.7f)
                {
                    Use_power("Laser_barrage");
                }
                //Use explosive_wave
                else
                {
                    Use_power("Explosive_wave");
                }

                yield return null;
            }
        }

        private void CalculateNextShot()
        {
            _timeNextShot = Time.timeSinceLevelLoad + Random.Range(_minDelayBetweenShots, _maxDelayBetweenShots);
        }

        private void Shoot()
        {
            var selectedBulletPrefab = _bulletPrefabs[Random.Range(0, _bulletPrefabs.Length)];
            var bullet_clone = PoolingSystem.Instance.Get<Bullet>(selectedBulletPrefab.name);
                               
            bullet_clone.transform.position = _bulletOrigin.position;
            bullet_clone.transform.right = _bulletOrigin.right;

            //Set
            bullet_clone.Configure_bullet(this.gameObject, _bulletCharge_value);
        }

        private void Use_power(string power)
        {
            //Call
            _superpowers.Use_super_power(power);
        }

        #endregion
    }
}
