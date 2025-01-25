using Game.Sounds;
using StarTravellers.Utils;
using System;
using UnityEngine;
using System.Collections;
using Game.Projectiles;
using Game.Haptics;
using Game.Particles;

public class Player : PausableBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _bulletOrigin;
    [Header("Setup")]
    [SerializeField] private float _speed;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private SpriteRenderer _mainRenderer;
    [SerializeField] private ParticleSystem _chargeParticles;

    private Rigidbody2D _rb;
    private Vector2 _velocity = Vector2.zero;
    public Damageable Damageable { get; private set; }

    private Material _mainMaterial;

    public float Life => Damageable.CurrentLife;

    private float _bullet_charge;

    #region Triggers

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.collider.tag)
        {
            case "Enemy":
                //Set
                Damageable.TakeDamage(1);
                break;
        }
    }

    #endregion

    private void Awake()
    {
        _shared.Player = this;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Damageable = GetComponent<Damageable>();
        Damageable.OnDie.AddListener(OnPlayerDie);
        Damageable.OnTakeDamage.AddListener(OnTakeDamage);
        _mainMaterial = _mainRenderer.material;
        _mainMaterial.SetFloat("_MaskStage", 0f);
        StopChargeParticleEmission();
    }

    private void OnTakeDamage()
    {
        // We don't want the player to take damage during wormhole transition
        if (_shared.LevelStage == GameplayStage.Wormhole)
        {
            return;
        }

        HapticsSystem.Instance.HighRumble(2f, .1f);
        StartCoroutine(AnimationHelpers.AnimationSequence(new Func<IEnumerator>[]
        {
                () => AnimationHelpers.SmoothLerp(f => _mainMaterial.SetFloat("_MaskStage",f),0,1,.05f),
                () => AnimationHelpers.SmoothLerp(f => _mainMaterial.SetFloat("_MaskStage",f),1,0,.1f),
        }));
    }
    private void OnPlayerDie()
    {
        // We don't want the player to take damage during wormhole transition
        if (_shared.LevelStage == GameplayStage.Wormhole)
        {
            return;
        }

        //Particles
        Particle_system.Create_particle("Debris_particle", transform.position);

        Destroy(gameObject);

        _shared.OnPlayerDie.Invoke();
    }

    protected override void DoUpdate()
    {
        // During wormhole player can't do anything
        if (_shared.LevelStage == GameplayStage.Wormhole)
        {
            return;
        }

        var levelModificators = _shared.CurrentLevel.Modificators;

        var vertical = Input.GetAxisRaw("Vertical") * levelModificators.Vertical;
        var horizontal = Input.GetAxisRaw("Horizontal") * levelModificators.Horizontal;

        _velocity = levelModificators.InvertControls ? new Vector2(vertical, horizontal)
                                                      : new Vector2(horizontal, vertical);


        // During boss presentation the player can't shoot
        if (_shared.LevelStage == GameplayStage.BossPresentation)
        {
            _bullet_charge = 0f;
            return;
        }

        if (!levelModificators.CantShoot)
        {
            //Inputs
            if (Input.GetButtonDown("Fire1"))
            {
                //Set
                _bullet_charge = 0;
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                StopChargeParticleEmission();
                Shoot();
            }

            if (Input.GetButton("Fire1"))
            {
                //Set
                _bullet_charge += Time.deltaTime;
            }            
        }

        // Bullet charge
        if (_bullet_charge >= .3f)
        {
            HapticsSystem.Instance.StartRumble(.5f * _bullet_charge, .5f * _bullet_charge);
            UpdateChargeParticleEmission();
        }
    }

    private void Shoot()
    {
        var bullet_clone = PoolingSystem.Instance.Get<Bullet>(_bulletPrefab.name);
        bullet_clone.transform.position = _bulletOrigin.position;

        //Set
        bullet_clone.Configure_bullet(this.gameObject, _bullet_charge);

        //Sound
        Sound_system.Create_sound("Laser_shoot", 0.3f, true);

        // Reset bullet charge
        _bullet_charge = 0f;
        HapticsSystem.Instance.StopRumble();
    }

    private void FixedUpdate()
    {
        var levelModificators = _shared.CurrentLevel.Modificators;
        _rb.linearVelocity = _velocity * _speed * levelModificators.SpeedScale;
    }

    private void StopChargeParticleEmission()
    {
        var emission = _chargeParticles.emission;
        emission.rateOverTime = 0f;
    }
    private void UpdateChargeParticleEmission()
    {
        var emission = _chargeParticles.emission;
        emission.rateOverTime = _bullet_charge * 10f;
    }
}
