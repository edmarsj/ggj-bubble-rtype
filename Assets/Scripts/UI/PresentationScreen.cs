using UnityEngine;

public class PresentationScreen : MonoBehaviour
{
    [SerializeField] private float _delay = 3f;
    private void Start()
    {
        Invoke(nameof(LoadNextScene),_delay);       
    }

    private void LoadNextScene()
    {
        TransitionController.Instance.TransitionToScene("Menu", false);
    }
}
