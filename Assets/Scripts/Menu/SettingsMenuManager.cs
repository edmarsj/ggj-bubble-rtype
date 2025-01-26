using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    [SerializeField] private Slider _sliderAudio;
    [SerializeField] private Button _btnReturnToMenu;    

    private void Start()
    {
        
        _sliderAudio.onValueChanged.AddListener(MasterVolumeChanged);
        _sliderAudio.value = PlayerPrefs.GetFloat("MasterVolume", .5f);
        _btnReturnToMenu.onClick.AddListener(() => TransitionController.Instance.TransitionToScene("Menu"));
    }

    private void MasterVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("MasterVolume", value);
        SettingsManager.Instance.Apply();
    }
}
