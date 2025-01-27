using System.Collections.Generic;
using UnityEngine;
using Game.Enemies;

[CreateAssetMenu(fileName = "Level", menuName = "Scriptable Objects/Level")]
public class Level : ScriptableObject
{
    [field: SerializeField] public string LevelName { get; set; }
    [field: SerializeField] public int PointsRequirement { get; set; }
    [field: SerializeField] public LevelModificators Modificators { get; set; } = new();
    [field: SerializeField] public float LevelScrollSpeed { get; set; } = .5f;
    [field: SerializeField] public List<SpawnerConfig> Spawners { get; set; } = new();
    [field: SerializeField] public BossBase BossPrefab { get; set; }
    [field: SerializeField] public Level NextLevel { get; set; }
}
