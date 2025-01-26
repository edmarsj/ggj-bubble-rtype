using Game.Particles;
using StarTravellers.Utils;
using System.Collections;
using UnityEngine;

public class Debries : PausableBehaviour
{
    [Header("Stats")]
    [SerializeField] private int _points;
    private bool _debryActivated = false;
    private Damageable _damageable;
    private Material _mainMaterial;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _damageable = GetComponent<Damageable>();        
        _damageable.OnDie.AddListener(OnDestroy);
        _damageable.OnTakeDamage.AddListener(OnTakeDamage);
        _damageable.IsPaused = true;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _mainMaterial = _spriteRenderer.material;
    }

    private void OnTakeDamage()
    {
        StartCoroutine(AnimationHelpers.AnimationSequence(new System.Func<IEnumerator>[]
        {
                () => AnimationHelpers.SmoothLerp(f => _mainMaterial.SetFloat("_MaskStage",f),0,1,.05f),
                () => AnimationHelpers.SmoothLerp(f => _mainMaterial.SetFloat("_MaskStage",f),1,0,.1f),
        }));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "EnemyActivator":
                _damageable.IsPaused = false;
                _debryActivated = true;
                break;
            case "Despawn":
                Destroy(gameObject);
                break;

            default:
                break;
        }
    }

    private void OnDestroy()
    {
        _shared.AddScore(_points);

        //Particles
        Particle_system.Create_particle("Debris_particle", transform.position);

        Destroy(gameObject);
    }
}
