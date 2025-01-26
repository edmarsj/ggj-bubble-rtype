using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] private Level _firstLevel;

    private void Awake()
    {
        PlayerPrefs.SetInt(_firstLevel.name, 1);
    }
}
