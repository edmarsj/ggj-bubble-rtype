using UnityEngine;

namespace Game.Gameplay
{
    public class KillPlayerOnCollision : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {           
            switch (collision.tag)
            {
                case "Player":
                    collision.GetComponent<Player>().Damageable.Kill();
                    break;                
            }
        }
    }
}
