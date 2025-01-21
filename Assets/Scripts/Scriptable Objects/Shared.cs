using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Shared", menuName = "Scriptable Objects/Shared")]
public class Shared : ScriptableObject
{
    [field: SerializeField] public bool IsPaused { get; set; }
    [field: SerializeField] public Level CurrentLevel { get; set; }
    [field: SerializeField] public int TotalPoints { get; set; }
    [field: SerializeField] public LevelStage LevelStage { get; set; } = LevelStage.Spawners;
    public int CurrentPointsOnLevel { get; set; }
    [field: SerializeField] public bool BossDefeated { get; set; }
    [field: SerializeField] public UnityEvent OnBeginBossBattle { get; set; }

    public BossBase Boss { get; set; }

    internal void AddScore(int points)
    {
        TotalPoints += points;
        CurrentPointsOnLevel += points;
    }
}
