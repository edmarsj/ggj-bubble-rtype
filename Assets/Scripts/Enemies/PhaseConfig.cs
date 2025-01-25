using UnityEngine;

namespace Game.Enemies
{
    [System.Serializable]
    public class PhaseConfig
    {
        [field: SerializeField] public float LifePerc { get; set; }
        [field: SerializeField] public GameObject[] SpecialPrefabs { get; set; }
    }
}