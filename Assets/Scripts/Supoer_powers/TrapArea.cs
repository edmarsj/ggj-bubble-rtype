using UnityEngine;

namespace Game.Super_powers
{
    /// <summary>
    /// Must be attached to a gameobject on the gameplay scene
    /// </summary>
    public class TrapArea : MonoBehaviour
    {
        public static TrapArea Instance { get; private set; }

        private Collider2D _collider2D;

        public Vector2 GetRandom2DPosition() => _collider2D.bounds.GetRandom2DPosition();

        private void Awake()
        {
            Instance = this;
            _collider2D = GetComponent<Collider2D>();
        }
    }

}