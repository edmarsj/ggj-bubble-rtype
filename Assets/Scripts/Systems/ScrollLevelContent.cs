using UnityEngine;

public class ScrollLevelContent : MonoBehaviour
{
    [SerializeField] private Shared _shared;

    private void Update()
    {
        transform.Translate(Vector2.left * _shared.CurrentLevel.LevelScrollSpeed * Time.deltaTime);
    }
}
