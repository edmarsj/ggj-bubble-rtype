using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [field: SerializeField] public float MaxLife { get; set; }
    [field: SerializeField] public UnityEvent OnDie { get; set; }
    private float _currentLife;

    [field: SerializeField] public bool Dead { get; set; }

    public float LifePercent => _currentLife / MaxLife;

    private void Start()
    {
        _currentLife = MaxLife;
    }

    public void TakeDamage(int amt)
    {
        if (Dead)
        {
            return;
        }

        _currentLife -= amt;

        if (_currentLife <= 0)
        {
            Dead = true;
            OnDie?.Invoke();
        }
    }
}
