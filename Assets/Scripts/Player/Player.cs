using Game.Sounds;
using StarTravellers.Utils;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.UIElements;
using System.Collections;
using Game.Projectiles;
using Game.Haptics;

public class Player : PausableBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _bulletOrigin;
    [Header("Setup")]
    [SerializeField] private float _speed;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private SpriteRenderer _mainRenderer;

    private Rigidbody2D _rb;
    private Vector2 _velocity = Vector2.zero;
    private Damageable _damageable;

    private Material _mainMaterial;

    public float Life => _damageable.CurrentLife;

    private float bullet_charge;

    #region Triggers

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.collider.tag)
        {
            case "Enemy":
                //Set
                _damageable.TakeDamage(1);
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
        _damageable = GetComponent<Damageable>();
        _damageable.OnDie.AddListener(OnPlayerDie);
        _damageable.OnTakeDamage.AddListener(OnTakeDamage);
        _mainMaterial = _mainRenderer.material;
        _mainMaterial.SetFloat("_MaskStage", 0f);
    }

    private void OnTakeDamage()
    {
        HapticsSystem.Instance.HighRumble(2f, .1f);
        StartCoroutine(AnimationHelpers.AnimationSequence(new Func<IEnumerator>[]
        {
                () => AnimationHelpers.SmoothLerp(f => _mainMaterial.SetFloat("_MaskStage",f),0,1,.05f),
                () => AnimationHelpers.SmoothLerp(f => _mainMaterial.SetFloat("_MaskStage",f),1,0,.1f),
        }));
    }
    private void OnPlayerDie()
    {
        SceneManager.LoadScene("GameOver");
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
            bullet_charge = 0f;
            return;
        }

        //Inputs
        if (Input.GetButtonDown("Fire1"))
        {
            //Set
            bullet_charge = 0;
        }
        else if (Input.GetButtonUp("Fire1"))
        {            
            //Call
            Shoot();
        }

        if (Input.GetButton("Fire1"))
        {
            //Set
            bullet_charge += Time.deltaTime;
           
        }
       
        // Bullet charge rumble
        if (bullet_charge >= .3f)
        {
            HapticsSystem.Instance.StartRumble(.5f * bullet_charge, .5f * bullet_charge);
        }
        
    }

    private void Shoot()
    {
        var bullet_clone = Instantiate(_bulletPrefab);
        bullet_clone.transform.position = _bulletOrigin.position;

        //Set
        bullet_clone.GetComponent<Bullet>().Configure_bullet(this.gameObject, bullet_charge);

        //Sound
        Sound_system.Create_sound("Laser_shoot", 0.3f, true);

        // Reset bullet charge
        bullet_charge = 0f;
        HapticsSystem.Instance.StopRumble();
    }

    private void FixedUpdate()
    {
        var levelModificators = _shared.CurrentLevel.Modificators;
        _rb.linearVelocity = _velocity * _speed * levelModificators.SpeedScale;
    }
}
