using System.Linq;
using UnityEngine;

public class EnemySpawner : PausableBehaviour
{
    [SerializeField] private SpawnerConfig.SpawnerId _spawnerId;

    private Bounds _boundaries;
    private SpawnerConfig _spawnerConfig;

    private float _timeNextSpawn = 0f;

    private void Start()
    {
        var colliderVolume = GetComponent<BoxCollider2D>();
        _boundaries = colliderVolume.bounds;
        _spawnerConfig = _shared.CurrentLevel.Spawners.FirstOrDefault(m => m.Id == _spawnerId);

        if (_spawnerConfig == null)
        {
            Destroy(gameObject);
        }

        CalculateNextSpawn();
    }

    private void CalculateNextSpawn()
    {
        _timeNextSpawn = Time.timeSinceLevelLoad + 60f / _spawnerConfig.RateMinute;
    }

    protected override void DoUpdate()
    {
        if (_shared.LevelStage == LevelStage.Spawners)
        {
            if (Time.timeSinceLevelLoad > _timeNextSpawn)
            {
                CalculateNextSpawn();


                var clone = Instantiate(_spawnerConfig.Prefab);
                clone.transform.position = _boundaries.GetRandom2DPosition();

            }
        }
    }
}
