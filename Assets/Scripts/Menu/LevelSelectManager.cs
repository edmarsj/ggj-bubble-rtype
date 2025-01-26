using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] private Level _firstLevel;
    [SerializeField] private Shared _shared;
    [SerializeField] private TMP_Text _points;
    [SerializeField] private Button _returnToMenuButton;

    private void Awake()
    {
        PlayerPrefs.SetInt(_firstLevel.name, 1);
    }

    private void OnEnable()
    {
        _returnToMenuButton.onClick.AddListener(() => TransitionController.Instance.TransitionToScene("Menu"));
    }


    private void Start()
    {
        _points.text = _shared.TotalPoints.ToString();
    }
}
