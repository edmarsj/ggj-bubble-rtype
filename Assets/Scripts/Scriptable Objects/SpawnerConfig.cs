using UnityEngine;

[System.Serializable]
public class SpawnerConfig
{
    public enum SpawnerId
    {
        Top,
        Mid,
        Bottom
    }

    [field: SerializeField] public SpawnerId Id { get; set; }
    [field: SerializeField] public int RateMinute { get; set; }
    [field: SerializeField] public GameObject Prefab { get; set; }
}
