using UnityEngine;
using UnityEngine.Events;
using Game.Enemies;

[CreateAssetMenu(fileName = "Shared", menuName = "Scriptable Objects/Shared")]
public class Shared : ScriptableObject
{
    [field: SerializeField] public bool IsPaused { get; set; }
    [field: SerializeField] public Level CurrentLevel { get; set; }
    [field: SerializeField] public int TotalPoints { get; set; }
    [field: SerializeField] public GameplayStage LevelStage { get; set; } = GameplayStage.Spawners;
    public int CurrentPointsOnLevel { get; set; }
    [field: SerializeField] public bool BossDefeated { get; set; }
    [field: SerializeField] public UnityEvent OnBeginBossBattle { get; set; }
    [field: SerializeField] public UnityEvent OnEndBossBattle { get; set; }
    [field: SerializeField] public UnityEvent Player_touch_worm_hole { get; set; }
    [field:SerializeField] public UnityEvent OnPlayerDie { get; set; }

    public BossBase Boss { get; set; }
    public Player Player { get; set; }    
    public Transform BossSpawnPosition { get; internal set; }

    internal void AddScore(int points)
    {
        //Set
        TotalPoints += points;
        CurrentPointsOnLevel += points;
    }

    [field: SerializeField] public UnityEvent FadeOutScreen { get; set; }
    [field: SerializeField] public UnityEvent FadeInScreen { get; set; }

}
