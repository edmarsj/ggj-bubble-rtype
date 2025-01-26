using UnityEngine;
using UnityEngine.Events;
using Game.Enemies;

[CreateAssetMenu(fileName = "Shared", menuName = "Scriptable Objects/Shared")]
public class Shared : ScriptableObject
{
    [field: SerializeField] public bool IsPaused { get; set; }
    [field: SerializeField] public Level CurrentLevel { get; set; }
    public LevelModificators CurrentModificators { get;  set; }

    public int TotalPoints
    {
        get { return PlayerPrefs.GetInt(nameof(TotalPoints), 0); }
        set { PlayerPrefs.SetInt(nameof(TotalPoints), value); }
    }
    public int CurrentPointsOnLevel { get; set; }
    [field: SerializeField] public GameplayStage LevelStage { get; set; } = GameplayStage.Spawners;
    
    [field: SerializeField] public bool BossDefeated { get; set; }
    [field: SerializeField] public UnityEvent OnBeginBossBattle { get; set; }
    [field: SerializeField] public UnityEvent OnEndBossBattle { get; set; }
    [field: SerializeField] public UnityEvent Player_touch_worm_hole { get; set; }
    [field: SerializeField] public UnityEvent OnPlayerDie { get; set; }

    public BossBase Boss { get; set; }
    public Player Player { get; set; }
    public Transform BossSpawnPosition { get; internal set; }

    internal void AddScore(int points)
    {
        //Set       
        CurrentPointsOnLevel += points;
    }

    public void ConfirmScore()
    {
        TotalPoints += CurrentPointsOnLevel;
    }

    [field: SerializeField] public UnityEvent FadeOutScreen { get; set; }
    [field: SerializeField] public UnityEvent FadeInScreen { get; set; }
   
    public void SetLevel(Level level)
    {
        CurrentLevel = level;
        CurrentModificators = level.Modificators.Copy();
        CurrentPointsOnLevel = 0;
    }

    public void RestoreModificators()
    {
        CurrentModificators = CurrentLevel.Modificators.Copy();
    }

}
