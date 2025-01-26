using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    public static SettingsManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Apply();
    }

    public void Apply()
    {
        _audioMixer.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume", .5f)) * 20);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void BeforeSceneLoad()
    {
        if (GameObject.Find("[SettingsManager]") == null)
        {
            GameObject main = Object.Instantiate(Resources.Load("Prefabs/SettingsManager")) as GameObject;
            main.name = "[SettingsManager]";
            DontDestroyOnLoad(main);
        }
    }
}
