using StarTravellers.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionController : MonoBehaviour
{

    public static TransitionController Instance { get; private set; }

    [SerializeField] private Shared _shared;
    [Header("Transition")]
    [SerializeField] private Image[] _transitionImages;

    private void Awake()
    {
        Instance = this;

        foreach (var image in _transitionImages)
        {
            image.fillAmount = 0f;
        }
    }

    private void OnEnable()
    {
        _shared.FadeInScreen.AddListener(OnFadeInScreen);
        _shared.FadeOutScreen.AddListener(OnFadeOutScreen);
    }

    private void OnDisable()
    {
        _shared.FadeInScreen.RemoveListener(OnFadeInScreen);
        _shared.FadeOutScreen.RemoveListener(OnFadeOutScreen);
    }

    private void OnFadeOutScreen()
    {
        StartCoroutine(AnimationHelpers.SmoothLerp(f =>
        {
            foreach (var image in _transitionImages)
            {
                image.fillAmount = f;
            }
        }, 0f, 1f, .5f));
    }

    private void OnFadeInScreen()
    {
        StartCoroutine(AnimationHelpers.SmoothLerp(f =>
        {
            foreach (var image in _transitionImages)
            {
                image.fillAmount = f;
            }
        }, 1f, 0f, .5f));
    }

    public void TransitionToScene(string sceneName, bool fadeOut = true)
    {
        if (fadeOut)
        {
            StartCoroutine(AnimationHelpers.AnimationSequence(new System.Func<IEnumerator>[]
              {
                      () => AnimationHelpers.Action(() => _shared.FadeOutScreen.Invoke()),
                      () => AnimationHelpers.Delay(.5f),
                      () => LoadSceneAsync(sceneName),
                      () => AnimationHelpers.Action(() => _shared.FadeInScreen.Invoke()),
              }));
        }
        else
        {
            StartCoroutine(AnimationHelpers.AnimationSequence(new System.Func<IEnumerator>[]
              {
                      () => LoadSceneAsync(sceneName),
                      () => AnimationHelpers.Action(() => _shared.FadeInScreen.Invoke()),
              }));
        }

    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void BeforeSceneLoad()
    {
        if (GameObject.Find("[Transitions]") == null)
        {
            GameObject main = Object.Instantiate(Resources.Load("Prefabs/Transitions")) as GameObject;
            main.name = "[Transitions]";
            DontDestroyOnLoad(main);
        }
    }
}
