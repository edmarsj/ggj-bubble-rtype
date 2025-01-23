using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoop : PausableBehaviour
{
    [SerializeField] private Transform _bossSpawnPoint;
    [SerializeField] private Level _firstLevel;
    private static bool _newGame = true;
    private void Awake()
    {       
        if (_newGame)
        {
            _shared.CurrentLevel = _firstLevel;
            _shared.TotalPoints = 0;
            _newGame = false;
        }

        //Set
        _shared.Player_touch_worm_hole.AddListener(Change_level);
    }

    private void Start()
    {
        _shared.LevelStage = LevelStage.Spawners;
        _shared.CurrentPointsOnLevel = 0;
        _shared.BossDefeated = false;
    }

    protected override void DoUpdate()
    {
        if (_shared.LevelStage == LevelStage.Spawners && _shared.CurrentPointsOnLevel >= _shared.CurrentLevel.PointsRequirement )
        {
            _shared.LevelStage = LevelStage.Boss;
            
            var boss = Instantiate(_shared.CurrentLevel.BossPrefab);
            boss.transform.position = _bossSpawnPoint.position;            
            _shared.Boss = boss;
            _shared.OnBeginBossBattle?.Invoke();
        }
    }

    private void Change_level()
    {
        //Set
        _shared.CurrentLevel = _shared.CurrentLevel.NextLevel;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
