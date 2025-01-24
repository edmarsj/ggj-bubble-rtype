using UnityEngine;

public class BeginBossBattle : MonoBehaviour
{
    [SerializeField] private Shared _shared;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _shared.ReachedBossTrigger = true;
        }        
    }
}
