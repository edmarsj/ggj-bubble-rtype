using UnityEngine;

namespace Game.Misc
{
    public class Move_gameObject : MonoBehaviour
    {
        [Range(0, 1f)]
        public float Speed;
        public Vector2 Dir;

        public void FixedUpdate()
        {
            //Set
            transform.Translate(Dir * Speed);
        }
    }
}
