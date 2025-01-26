using UnityEngine;
using UnityEngine.Events;

namespace Game.Gameplay
{
    public class TriggerEventOnCollision : MonoBehaviour
    {

        [field: Header("Trigger events on collision only")]
        [field: SerializeField] public UnityEvent OnTriggerEnter { get; set; }

        [field: Header("Reset on start and trigger on collision")]
        [field: SerializeField] public UnityEvent<bool> OnTriggerEnterOrReset { get; set; }
        [SerializeField] private bool _triggerOnce = true;


        [Space]
        [Header("Debug")]
        [SerializeField] private Color _debugColor;
        private bool _triggered = false;

        private void Start()
        {
            OnTriggerEnterOrReset?.Invoke(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_triggered)
            {
                return;
            }

            if (collision.CompareTag("Player"))
            {
                if (_triggerOnce)
                {
                    _triggered = true;
                    OnTriggerEnter?.Invoke();
                    OnTriggerEnterOrReset?.Invoke(true);
                 
                    // Deleta apenas o componente, deixando o GO intacto.
                    Destroy(this);
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (TryGetComponent<Collider2D>(out var collider))
            {
                Gizmos.color = _debugColor;
                Gizmos.DrawCube(collider.bounds.center, collider.bounds.size);
            }
        }
#endif
    }
}