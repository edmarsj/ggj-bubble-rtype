using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    [SerializeField] private Shared _shared;
    [SerializeField] private Level _level;

    private Button _button;
    private TMP_Text _title;

    private void Start()
    {
        EnsureReferences();
        _button.onClick.AddListener(StartSelectedLevel);

        var unlocked = PlayerPrefs.GetInt(_level.name, 0) == 1;
        _button.interactable = unlocked;
    }

    private void OnValidate()
    {
        EnsureReferences();
        if (_level)
        {
            name = $"Button_level_{_level.name}";
        }
    }

    private void EnsureReferences()
    {
        _button = GetComponent<Button>();
        _title = GetComponentInChildren<TMP_Text>();
        if (_level)
        {
            _title.text = _level.LevelName;
        }
    }

    private void StartSelectedLevel()
    {
        _shared.SetLevel(_level);
        SceneManager.LoadScene("Gameplay");
    }
}
