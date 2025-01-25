using Game.Haptics;
using StarTravellers.Utils;
using UnityEngine;

namespace Game.Super_powers
{
    public class WormholeBubble : TrapBase
    {
        
        [Header("Settings")]
        [SerializeField] private Transform _wormholeEntrance;
        [SerializeField] private Transform _wormholeExit;
       
        
        private bool _exitPositionAvailable = false;
        private Collider2D _collider2D;
        private bool _playerTraped = false;

        private void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
            _wormholeEntrance.localScale = Vector3.zero;
            _wormholeExit.localScale = Vector3.zero;

        }

        private void Start()
        {
            // This component itself will be wormhole trap!
            transform.parent = TrapArea.Instance.transform;            
            StartCoroutine(AnimationHelpers.SmoothTransformScale(_wormholeEntrance, Vector3.zero, Vector3.one, .3f));
        }

        protected override void DoUpdate()
        {
            if (!TrySetTrap())
            {
                return;
            }

            // Lets search each frame for a teleport position, it can't be within the current boundaries of the object
            // We do on a frame basis to not cause bottlonecks 
            if (!_exitPositionAvailable)
            {
                var testPosition = TrapArea.Instance.GetRandom2DPosition();

                if (Vector2.Distance(transform.position, testPosition) >= _minTrapDistance)
                {
                    _wormholeExit.position = testPosition;
                    _exitPositionAvailable = true;
                }
            }
        }

        private void FixedUpdate()
        {
            if (_playerTraped)
            {
                var rb = _shared.Player.GetComponent<Rigidbody2D>();
                rb.position = _wormholeExit.position;
                _playerTraped = false;
                StartCoroutine(AnimationHelpers.SmoothTransformScale(_wormholeExit, Vector3.zero, Vector3.one, .3f));
                Invoke(nameof(DespawnDelayed), 1f);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_trapSet)
            {
                return;
            }

            if (!_playerTraped && collision.CompareTag("Player"))
            {
                StartCoroutine(AnimationHelpers.SmoothTransformScale(_wormholeEntrance, Vector3.one, Vector3.zero, .3f));
                _currentTraps.Remove(transform);
                _playerTraped = true;
                HapticsSystem.Instance.Pulse(3f);
            }
        }

        private void DespawnDelayed()
        {
            StartCoroutine(AnimationHelpers.SmoothTransformScale(_wormholeExit, Vector3.one, Vector3.zero, .3f));            
            Destroy(gameObject, .3f);
        }



    }

}