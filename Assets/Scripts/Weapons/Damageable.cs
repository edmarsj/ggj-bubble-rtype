using Game.Sounds;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [field: SerializeField] public float MaxLife { get; set; }
    [field: SerializeField] public UnityEvent OnDie { get; set; }
    public float CurrentLife { get; private set; }

    [field: SerializeField] public bool Dead { get; set; }

    public float LifePercent => CurrentLife / MaxLife;

    private void Start()
    {
        CurrentLife = MaxLife;
    }

    public void TakeDamage(int amt)
    {
        if (Dead)
        {
            return;
        }

        CurrentLife -= amt;

        if (CurrentLife <= 0)
        {
            Dead = true;
            OnDie?.Invoke();
        }

        //Sound
        Sound_system.Create_sound("Retro_impact", 0.2f);
    }
}
