using UnityEngine;
using UnityEngine.Events;
using Game.Enemies;

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
    [field: SerializeField] public UnityEvent OnEndBossBattle { get; set; }
    [field: SerializeField] public UnityEvent Player_touch_worm_hole { get; set; }

    public BossBase Boss { get; set; }
    public Player Player { get; set; }

    internal void AddScore(int points)
    {
        //Set
        TotalPoints += points;
        CurrentPointsOnLevel += points;
    }
}
