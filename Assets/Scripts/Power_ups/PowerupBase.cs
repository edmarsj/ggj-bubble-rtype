using System;
using UnityEngine;

namespace Game.PowerUps
{
    public class PowerupBase : PausableBehaviour
    {
        [SerializeField] float _ttl;
        private Rigidbody2D _rbd2;

        #region Triggers

        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (collision.tag)
            {
                case "Player":
                    ConsumePowerUp(collision.GetComponent<Player>());
                    Destroy(this.gameObject);
                    break;

                case "Enemy":
                    Destroy(this.gameObject);
                    break;
            }
        }

        #endregion

        private void Awake()
        {
            _rbd2 = GetComponent<Rigidbody2D>();
            _ttl = Time.timeSinceLevelLoad + _ttl;
        }

        #region Powerups actions
        private void ConsumePowerUp(Player player)
        {
            player.Damageable.AddLife(1);
        }
        #endregion

        #region Core

        protected override void OnPause()
        {
            _rbd2.linearVelocity = Vector2.zero;
        }

        protected override void DoUpdate()
        {

            if (Time.timeSinceLevelLoad > _ttl)
            {
                Destroy(gameObject);
            }
        }

        #endregion
    }
}
