using Game.Sounds;
using System;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [field: SerializeField] public float MaxLife { get; set; }
    [field: SerializeField] public UnityEvent OnDie { get; set; }
    [field: SerializeField] public UnityEvent OnTakeDamage { get; set; }
    public float CurrentLife { get; private set; }
    [field: SerializeField] public bool IsPaused { get; set; } = false;

    [field: SerializeField] public bool Dead { get; set; }

    public float LifePercent => CurrentLife / MaxLife;

    private void Start()
    {
        CurrentLife = MaxLife;
    }

    public void TakeDamage(float amt)
    {
        if (IsPaused)
        {
            return;
        }
        if (Dead)
        {
            return;
        }

        OnTakeDamage?.Invoke();
        CurrentLife -= amt;

        if (CurrentLife <= 0)
        {
            Kill();
        }

        //Sound
        Sound_system.Create_sound("Retro_impact", 0.2f);
    }

    public void AddLife(int amt)
    {
        CurrentLife = Mathf.Min(MaxLife, CurrentLife + amt);
    }

    public void Kill()
    {
        Dead = true;
        OnDie?.Invoke();
    }
}
