using UnityEngine;

[System.Serializable]
public class LevelModificators
{
    [field: SerializeField] public float Vertical { get; set; } = 1f;
    [field: SerializeField] public float Horizontal { get; set; } = 1f;
    [field: SerializeField] public bool InvertControls { get; set; } = false;
    [field: SerializeField] public float SpeedScale { get; set; } = 1f;
}
