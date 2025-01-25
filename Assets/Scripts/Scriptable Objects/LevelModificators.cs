using UnityEngine;

[System.Serializable]
public class LevelModificators
{
    [field: SerializeField] public float Vertical { get; set; } = 1f;
    [field: SerializeField] public float Horizontal { get; set; } = 1f;
    [field: SerializeField] public bool InvertControls { get; set; } = false;
    [field: SerializeField] public float SpeedScale { get; set; } = 1f;
    public bool CantShoot { get; set; } = false;

    public LevelModificators Copy()
    {
        return new()
        {
            Vertical = Vertical,
            Horizontal = Horizontal,
            InvertControls = InvertControls,
            SpeedScale = SpeedScale,
            CantShoot = CantShoot
        };
    }

    public void Set(LevelModificators levelModificators)
    {
        Vertical = levelModificators.Vertical;
        Horizontal = levelModificators.Horizontal;
        InvertControls = levelModificators.InvertControls;
        SpeedScale = levelModificators.SpeedScale;
        CantShoot = levelModificators.CantShoot;
    }
}
