using Game.Haptics;
using StarTravellers.Utils;
using UnityEngine;

namespace Game.Super_powers
{
    public class CantShootBubbleTrap : TrapBase
    {
        private bool _playerTraped = false;

        [Header("Settings")]
        /// <summary>
        /// Stays on field for x seconds
        /// </summary>
        [SerializeField] private float _duration = 10f;                
        [SerializeField] private Transform _root;
        [SerializeField] private SpriteRenderer _mainRenderer;

        private LevelModificators _realLevelModificators;

        private void Start()
        {
            transform.parent = TrapArea.Instance.transform;
            _realLevelModificators = _shared.CurrentLevel.Modificators.Copy();
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
                _shared.CurrentLevel.Modificators.CantShoot = true;
                HapticsSystem.Instance.StartRumble(.5f, .5f);
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (_playerTraped)
            {
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
            _shared.CurrentLevel.Modificators.Set(_realLevelModificators);
            HapticsSystem.Instance.StopRumble();
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