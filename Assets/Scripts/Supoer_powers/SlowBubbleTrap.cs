using Game.Haptics;
using StarTravellers.Utils;
using UnityEngine;

namespace Game.Super_powers
{
    public class SlowBubbleTrap : TrapBase
    {
        private bool _playerTraped = false;

        [Header("Settings")]
        /// <summary>
        /// Stays on field for x seconds
        /// </summary>
        [SerializeField] private float _duration = 10f;
        [SerializeField] private float _valueOuterCollider = .1f;
        [SerializeField] private float _valueInnerCollider = .07f;
        [SerializeField] private Transform _root;
        [SerializeField] private SpriteRenderer _mainRenderer;
        [SerializeField] private Collider2D _innerCollider;


        private void Start()
        {
            transform.parent = TrapArea.Instance.transform;            
            StartCoroutine(AnimationHelpers.SmoothTransformScale(_root, Vector3.zero, Vector3.one, .3f));
            Invoke(nameof(DestroyTrap), _duration);
        }

        private void OnDisable()
        {
            if (_playerTraped)
            {
                RestoreLevel();
            }
        }

        protected override void DoUpdate()
        {
            if (!TrySetTrap())
            {
                return;
            }

            if (_playerTraped)
            {

            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_trapSet)
            {
                return;
            }

            if (collision.CompareTag("Player"))
            {
                _playerTraped = true;                
            }

        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (_playerTraped && collision.CompareTag("Player"))
            {
                var levelModificators = _shared.CurrentModificators;
                if (_innerCollider.OverlapPoint(collision.transform.position))
                {
                    levelModificators.Horizontal = _valueInnerCollider;
                    levelModificators.Vertical = _valueOuterCollider;
                }
                else
                {
                    levelModificators.Horizontal = _valueOuterCollider;
                    levelModificators.Vertical = _valueOuterCollider;
                }

                HapticsSystem.Instance.Pulse(2f);
            }
        }


        private void OnTriggerExit2D(Collider2D collision)
        {
            if (_playerTraped && collision.CompareTag("Player"))
            {
                RestoreLevel();
                _playerTraped = false;
            }
        }     

        private void RestoreLevel()
        {
            _shared.RestoreModificators();    
        }

        private void DestroyTrap()
        {
            if (_playerTraped)
            {
                RestoreLevel();
            }

            StartCoroutine(AnimationHelpers.SmoothLerp(f =>
            {
                _root.localScale = Vector3.one * (1f + f);
                var color = _mainRenderer.color;
                color.a = 1 - f;
                _mainRenderer.color = color;
            }, 0f, 1f, .3f));
            _currentTraps.Remove(transform);
            Destroy(gameObject, .3f);
        }
    }

}